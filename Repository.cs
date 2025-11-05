using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Npgsql;

namespace OpenFood;

public class Repository
{
    private readonly DatabaseContext _dbContext;
    private readonly Config _config;
    private readonly object _progressLock = new();
    private long _totalRowsUploaded = 0;
    private int _successfulUploads = 0;

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

        // Create temp table (auto-dropped when connection closes)
        var tempTableName = $"temp_{Guid.NewGuid():N}";
        var columnDefs = string.Join(", ", Product.Columns.Select(c => $"\"{c.Name}\" {c.Type}"));
        await using (var cmd = new NpgsqlCommand($"CREATE TEMP TABLE {tempTableName} ({columnDefs})", conn))
        {
            await cmd.ExecuteNonQueryAsync();
        }

        // COPY into temp table
        var copyCommand = $"COPY {tempTableName} ({string.Join(", ", Product.ColumnNames.Select(c => $"\"{c}\""))}) FROM STDIN (FORMAT TEXT, DELIMITER E'\\t', NULL '\\N')";

        await using (var writer = await conn.BeginTextImportAsync(copyCommand))
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, config);

            while (await csv.ReadAsync())
            {
                var values = new List<string>();
                for (int i = 0; i < Product.ColumnNames.Length; i++)
                {
                    var value = csv.GetField(i);

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
            }
        }

        // Insert with ON CONFLICT UPDATE (upsert - keep newest)
        // Use DISTINCT ON to handle duplicates within the temp table
        var columnList = string.Join(", ", Product.ColumnNames.Select(c => $"\"{c}\""));
        var updateList = string.Join(", ", Product.ColumnNames.Where(c => c != "code").Select(c => $"\"{c}\" = EXCLUDED.\"{c}\""));
        await using (var cmd = new NpgsqlCommand($"INSERT INTO products ({columnList}) SELECT DISTINCT ON (\"code\") {columnList} FROM {tempTableName} ORDER BY \"code\" ON CONFLICT (\"code\") DO UPDATE SET {updateList}", conn))
        {
            await cmd.ExecuteNonQueryAsync();
        }

        var duration = (DateTime.Now - fileStart).TotalSeconds;

        lock (_progressLock)
        {
            _totalRowsUploaded += rowCount;
            _successfulUploads++;
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [{fileInfo.Name}] Loaded {rowCount:N0} rows");
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
}
