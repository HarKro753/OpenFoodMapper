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
    private readonly Dictionary<string, int> _categoryCache = new();
    private readonly object _categoryCacheLock = new();
    private readonly Dictionary<string, int> _additiveCache = new();
    private readonly object _additiveCacheLock = new();

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
        var productCategoriesRaw = new List<(string ProductCode, string CategoriesEn)>();
        var productAdditivesRaw = new List<(string ProductCode, string AdditivesEn)>();

        await using (var writer = await conn.BeginTextImportAsync(copyCommand))
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, config);

            // Use CsvSchema.AllColumns to get correct indices from the CSV file
            var categoriesEnIndex = Array.IndexOf(CsvSchema.AllColumns, "categories_en");
            var additivesEnIndex = Array.IndexOf(CsvSchema.AllColumns, "additives_en");
            var codeIndex = Array.IndexOf(CsvSchema.AllColumns, "code");

            // Create a mapping from Product columns to CSV indices
            var productColumnIndices = new Dictionary<string, int>();
            foreach (var col in Product.ColumnNames)
            {
                if (!excludedColumns.Contains(col))
                {
                    productColumnIndices[col] = Array.IndexOf(CsvSchema.AllColumns, col);
                }
            }

            while (await csv.ReadAsync())
            {
                var values = new List<string>();
                string? productCode = null;
                string? categoriesEn = null;
                string? additivesEn = null;

                // Read special columns for relations
                if (categoriesEnIndex >= 0)
                {
                    var value = csv.GetField(categoriesEnIndex);
                    if (!string.IsNullOrWhiteSpace(value) && value != "N")
                    {
                        categoriesEn = value;
                    }
                }
                if (additivesEnIndex >= 0)
                {
                    var value = csv.GetField(additivesEnIndex);
                    if (!string.IsNullOrWhiteSpace(value) && value != "N")
                    {
                        additivesEn = value;
                    }
                }
                if (codeIndex >= 0)
                {
                    var value = csv.GetField(codeIndex);
                    if (!string.IsNullOrEmpty(value) && value != "N")
                    {
                        productCode = value;
                    }
                }

                // Read only the columns we want to store, in the correct order
                foreach (var colName in productColumnNames)
                {
                    var csvIndex = productColumnIndices[colName];
                    var value = csv.GetField(csvIndex);

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

                if (!string.IsNullOrWhiteSpace(productCode) && !string.IsNullOrWhiteSpace(categoriesEn))
                {
                    productCategoriesRaw.Add((productCode, categoriesEn));
                }
                if (!string.IsNullOrWhiteSpace(productCode) && !string.IsNullOrWhiteSpace(additivesEn))
                {
                    productAdditivesRaw.Add((productCode, additivesEn));
                }
            }
        }

        var productCategories = new List<(string ProductCode, int CategoryId)>();
        foreach (var (productCode, categoriesEn) in productCategoriesRaw)
        {
            var categoryId = await ProcessCategoryHierarchyAsync(conn, categoriesEn);
            if (categoryId.HasValue)
            {
                productCategories.Add((productCode, categoryId.Value));
            }
        }

        var productAdditives = new List<(string ProductCode, int AdditiveId)>();
        foreach (var (productCode, additivesEn) in productAdditivesRaw)
        {
            var additiveIds = await ProcessAdditivesAsync(conn, additivesEn);
            foreach (var additiveId in additiveIds)
            {
                productAdditives.Add((productCode, additiveId));
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
                    "INSERT INTO product_categories (\"product_code\", \"category_id\") VALUES (@code::NUMERIC, @catId) ON CONFLICT DO NOTHING",
                    conn);
                cmd.Parameters.AddWithValue("code", productCode);
                cmd.Parameters.AddWithValue("catId", categoryId);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        if (productAdditives.Count > 0)
        {
            foreach (var (productCode, additiveId) in productAdditives)
            {
                await using var cmd = new NpgsqlCommand(
                    "INSERT INTO product_additives (\"product_code\", \"additive_id\") VALUES (@code::NUMERIC, @addId) ON CONFLICT DO NOTHING",
                    conn);
                cmd.Parameters.AddWithValue("code", productCode);
                cmd.Parameters.AddWithValue("addId", additiveId);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        var duration = (DateTime.Now - fileStart).TotalSeconds;

        lock (_progressLock)
        {
            _totalRowsUploaded += rowCount;
            _successfulUploads++;
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [{fileInfo.Name}] Loaded {rowCount:N0} rows, {productCategories.Count:N0} category mappings, {productAdditives.Count:N0} additive mappings");
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

        var broadestCategory = categoryNames[0].Trim();

        if (string.IsNullOrWhiteSpace(broadestCategory))
            return null;

        var cleanedName = CleanCategoryName(broadestCategory);

        if (string.IsNullOrWhiteSpace(cleanedName))
            return null;

        var categoryId = await GetOrCreateCategoryAsync(conn, cleanedName);
        return categoryId;
    }

    private string CleanCategoryName(string name)
    {
        if (name.Length > 3 && name[2] == ':')
        {
            name = name.Substring(3);
        }

        name = name.Replace('-', ' ');

        return name.Trim();
    }

    private async Task<int> GetOrCreateCategoryAsync(NpgsqlConnection conn, string name)
    {
        lock (_categoryCacheLock)
        {
            if (_categoryCache.TryGetValue(name, out var cachedId))
                return cachedId;
        }

        var selectSql = "SELECT \"id\" FROM categories WHERE \"name\" = @name";

        await using (var cmd = new NpgsqlCommand(selectSql, conn))
        {
            cmd.Parameters.AddWithValue("name", name);

            var result = await cmd.ExecuteScalarAsync();
            if (result != null)
            {
                var id = Convert.ToInt32(result);
                lock (_categoryCacheLock)
                {
                    _categoryCache[name] = id;
                }
                return id;
            }
        }

        var insertSql = "INSERT INTO categories (\"name\") VALUES (@name) ON CONFLICT (\"name\") DO UPDATE SET \"name\" = EXCLUDED.\"name\" RETURNING \"id\"";

        await using (var cmd = new NpgsqlCommand(insertSql, conn))
        {
            cmd.Parameters.AddWithValue("name", name);

            var newId = Convert.ToInt32(await cmd.ExecuteScalarAsync() ?? throw new Exception("Failed to insert category"));
            lock (_categoryCacheLock)
            {
                _categoryCache[name] = newId;
            }
            return newId;
        }
    }

    private async Task<List<int>> ProcessAdditivesAsync(NpgsqlConnection conn, string additivesEn)
    {
        var additiveIds = new List<int>();
        var additiveNames = additivesEn.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        foreach (var additiveName in additiveNames)
        {
            var trimmedName = additiveName.Trim();

            if (string.IsNullOrWhiteSpace(trimmedName))
                continue;

            var additiveId = await GetOrCreateAdditiveAsync(conn, trimmedName);
            additiveIds.Add(additiveId);
        }

        return additiveIds;
    }

    private async Task<int> GetOrCreateAdditiveAsync(NpgsqlConnection conn, string name)
    {
        lock (_additiveCacheLock)
        {
            if (_additiveCache.TryGetValue(name, out var cachedId))
                return cachedId;
        }

        var selectSql = "SELECT \"id\" FROM additives WHERE \"name\" = @name";

        await using (var cmd = new NpgsqlCommand(selectSql, conn))
        {
            cmd.Parameters.AddWithValue("name", name);

            var result = await cmd.ExecuteScalarAsync();
            if (result != null)
            {
                var id = Convert.ToInt32(result);
                lock (_additiveCacheLock)
                {
                    _additiveCache[name] = id;
                }
                return id;
            }
        }

        var insertSql = "INSERT INTO additives (\"name\") VALUES (@name) ON CONFLICT (\"name\") DO UPDATE SET \"name\" = EXCLUDED.\"name\" RETURNING \"id\"";

        await using (var cmd = new NpgsqlCommand(insertSql, conn))
        {
            cmd.Parameters.AddWithValue("name", name);

            var newId = Convert.ToInt32(await cmd.ExecuteScalarAsync() ?? throw new Exception("Failed to insert additive"));
            lock (_additiveCacheLock)
            {
                _additiveCache[name] = newId;
            }
            return newId;
        }
    }
}
