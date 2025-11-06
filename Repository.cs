using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Npgsql;
using OpenFood.Models;

namespace OpenFood;

public class Repository
{
    private readonly DatabaseContext _dbContext;
    private readonly Config _config;
    private readonly object _progressLock = new();
    private long _totalRowsUploaded = 0;
    private int _successfulUploads = 0;
    private readonly Dictionary<(string Name, int? ParentId), int> _categoryCache = new();
    private readonly object _categoryCacheLock = new();

    public Repository(DatabaseContext dbContext, Config config)
    {
        _dbContext = dbContext;
        _config = config;
    }

    public async Task<(bool Success, string FileName)> UploadFileAsync(string filePath, int fileNum, int totalFiles)
    {
        try
        {
            await using var conn = await _dbContext.OpenConnectionAsync();
            await UploadFileInternalAsync(conn, filePath, fileNum, totalFiles);
            return (true, Path.GetFileName(filePath));
        }
        catch (Exception ex)
        {
            lock (_progressLock)
            {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}]  ERROR uploading {Path.GetFileName(filePath)}: {ex.Message}");
            }
            return (false, Path.GetFileName(filePath));
        }
    }

    private async Task<(long RowCount, double Duration)> UploadFileInternalAsync(
        NpgsqlConnection conn, string filePath, int fileNum, int totalFiles)
    {
        var fileStart = DateTime.Now;
        var fileInfo = new FileInfo(filePath);
        var fileSizeMb = fileInfo.Length / (1024.0 * 1024.0);

        lock (_progressLock)
        {
            Console.WriteLine($"\n[{DateTime.Now:HH:mm:ss}] Processing file {fileNum}/{totalFiles}: {fileInfo.Name}");
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] File size: {fileSizeMb:F2} MB");
        }

        long rowCount = 0;
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = "\t",
            HasHeaderRecord = false,
            BadDataFound = null,
            MissingFieldFound = null
        };

        var excludedColumns = new[] { "categories", "categories_tags", "categories_en" };
        var productColumns = Product.Columns.Where(c => !excludedColumns.Contains(c.Name)).ToArray();
        var productColumnNames = Product.ColumnNames.Where(c => !excludedColumns.Contains(c)).ToArray();

        var tempTableName = $"temp_{Guid.NewGuid():N}";
        var columnDefs = string.Join(", ", productColumns.Select(c => $"\"{c.Name}\" {c.Type}"));
        await using (var cmd = new NpgsqlCommand($"CREATE TEMP TABLE {tempTableName} ({columnDefs})", conn))
        {
            await cmd.ExecuteNonQueryAsync();
        }

        var copyCommand = $"COPY {tempTableName} ({string.Join(", ", productColumnNames.Select(c => $"\"{c}\""))}) FROM STDIN (FORMAT TEXT, DELIMITER E'\\t', NULL '\\N')";
        var productCategoriesRaw = new List<(long ProductCode, string CategoriesEn)>();

        await using (var writer = await conn.BeginTextImportAsync(copyCommand))
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, config);

            var categoriesEnIndex = Array.IndexOf(Product.ColumnNames, "categories_en");
            var codeIndex = Array.IndexOf(Product.ColumnNames, "code");

            while (await csv.ReadAsync())
            {
                var values = new List<string>();
                long? productCode = null;
                string? categoriesEn = null;

                for (int i = 0; i < Product.ColumnNames.Length; i++)
                {
                    var value = csv.GetField(i);
                    var columnName = Product.ColumnNames[i];

                    if (i == categoriesEnIndex && !string.IsNullOrWhiteSpace(value) && value != "N")
                    {
                        categoriesEn = value;
                    }
                    if (i == codeIndex && !string.IsNullOrEmpty(value) && value != "N")
                    {
                        if (long.TryParse(value, out var code))
                            productCode = code;
                    }

                    if (excludedColumns.Contains(columnName))
                        continue;

                    if (string.IsNullOrEmpty(value) || value == "N")
                    {
                        values.Add("\\N");
                    }
                    else
                    {
                        value = value.Replace("\\", "\\\\")
                                    .Replace("\t", "\\t")
                                    .Replace("\n", "\\n")
                                    .Replace("\r", "\\r");
                        values.Add(value);
                    }
                }

                await writer.WriteLineAsync(string.Join("\t", values));
                rowCount++;

                if (productCode.HasValue && !string.IsNullOrWhiteSpace(categoriesEn))
                {
                    productCategoriesRaw.Add((productCode.Value, categoriesEn));
                }
            }
        }

        var productCategories = new List<(long ProductCode, int CategoryId)>();
        foreach (var (productCode, categoriesEn) in productCategoriesRaw)
        {
            var categoryId = await ProcessCategoryHierarchyAsync(conn, categoriesEn);
            if (categoryId.HasValue)
            {
                productCategories.Add((productCode, categoryId.Value));
            }
        }

        var columnList = string.Join(", ", productColumnNames.Select(c => $"\"{c}\""));
        var updateList = string.Join(", ", productColumnNames.Where(c => c != "code").Select(c => $"\"{c}\" = EXCLUDED.\"{c}\""));
        await using (var cmd = new NpgsqlCommand($"INSERT INTO products ({columnList}) SELECT DISTINCT ON (\"code\") {columnList} FROM {tempTableName} ORDER BY \"code\" ON CONFLICT (\"code\") DO UPDATE SET {updateList}", conn))
        {
            await cmd.ExecuteNonQueryAsync();
        }

        if (productCategories.Count > 0)
        {
            foreach (var (productCode, categoryId) in productCategories)
            {
                await using var cmd = new NpgsqlCommand(
                    "INSERT INTO product_categories (\"product_code\", \"category_id\") VALUES (@code, @catId) ON CONFLICT DO NOTHING",
                    conn);
                cmd.Parameters.AddWithValue("code", productCode);
                cmd.Parameters.AddWithValue("catId", categoryId);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        var duration = (DateTime.Now - fileStart).TotalSeconds;

        lock (_progressLock)
        {
            _totalRowsUploaded += rowCount;
            _successfulUploads++;
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [{fileInfo.Name}] Loaded {rowCount:N0} rows, {productCategories.Count:N0} category mappings");
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}]  [{fileInfo.Name}] Uploaded successfully in {duration:F1}s");
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Progress: {_successfulUploads}/{totalFiles} files | {_totalRowsUploaded:N0} total rows");
        }

        return (rowCount, duration);
    }

    public (long TotalRows, int SuccessfulFiles) GetStats()
    {
        lock (_progressLock)
        {
            return (_totalRowsUploaded, _successfulUploads);
        }
    }

    public int GetCategoryCount()
    {
        lock (_categoryCacheLock)
        {
            return _categoryCache.Count;
        }
    }

    private async Task<int?> ProcessCategoryHierarchyAsync(NpgsqlConnection conn, string categoriesEn)
    {
        var categoryNames = categoriesEn.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (categoryNames.Length == 0)
            return null;

        int? parentId = null;
        int? leafCategoryId = null;

        foreach (var categoryName in categoryNames)
        {
            var trimmedName = categoryName.Trim();

            if (string.IsNullOrWhiteSpace(trimmedName))
                continue;

            var categoryId = await GetOrCreateCategoryAsync(conn, trimmedName, parentId);
            leafCategoryId = categoryId;

            parentId = categoryId;
        }

        return leafCategoryId;
    }

    private async Task<int> GetOrCreateCategoryAsync(NpgsqlConnection conn, string name, int? parentId)
    {
        var cacheKey = (name, parentId);

        lock (_categoryCacheLock)
        {
            if (_categoryCache.TryGetValue(cacheKey, out var cachedId))
                return cachedId;
        }

        var selectSql = parentId.HasValue
            ? "SELECT \"id\" FROM categories WHERE \"name\" = @name AND \"parent_id\" = @parentId"
            : "SELECT \"id\" FROM categories WHERE \"name\" = @name AND \"parent_id\" IS NULL";

        await using (var cmd = new NpgsqlCommand(selectSql, conn))
        {
            cmd.Parameters.AddWithValue("name", name);
            if (parentId.HasValue)
                cmd.Parameters.AddWithValue("parentId", parentId.Value);

            var result = await cmd.ExecuteScalarAsync();
            if (result != null)
            {
                var id = Convert.ToInt32(result);
                lock (_categoryCacheLock)
                {
                    _categoryCache[cacheKey] = id;
                }
                return id;
            }
        }

        var insertSql = parentId.HasValue
            ? "INSERT INTO categories (\"name\", \"parent_id\") VALUES (@name, @parentId) RETURNING \"id\""
            : "INSERT INTO categories (\"name\", \"parent_id\") VALUES (@name, NULL) RETURNING \"id\"";

        await using (var cmd = new NpgsqlCommand(insertSql, conn))
        {
            cmd.Parameters.AddWithValue("name", name);
            if (parentId.HasValue)
                cmd.Parameters.AddWithValue("parentId", parentId.Value);

            var newId = Convert.ToInt32(await cmd.ExecuteScalarAsync() ?? throw new Exception("Failed to insert category"));
            lock (_categoryCacheLock)
            {
                _categoryCache[cacheKey] = newId;
            }
            return newId;
        }
    }
}
