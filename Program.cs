namespace OpenFood;

class Program
{
    static async Task Main()
    {
        var startTime = DateTime.Now;

        var config = new Config();
        var dbContext = new DatabaseContext(config);
        var repository = new Repository(dbContext, config);

        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Starting parallel upload process...");
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Using {config.MaxWorkers} parallel workers");

        try
        {
            var csvFiles = Directory.GetFiles(config.DataFolder, "part_*")
                .OrderBy(f => f)
                .ToArray();

            if (csvFiles.Length == 0)
            {
                Console.WriteLine($"ERROR: No files found in {config.DataFolder}/ folder!");
                return;
            }

            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Found {csvFiles.Length} files to upload");
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Files: {Path.GetFileName(csvFiles[0])} to {Path.GetFileName(csvFiles[^1])}");
            Console.WriteLine(new string('=', 80));

            // Create table and upload first file
            Console.WriteLine($"\n[{DateTime.Now:HH:mm:ss}] Using predefined column names ({Product.ColumnNames.Length} columns)");
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Processing first file to create table: {Path.GetFileName(csvFiles[0])}");

            var firstFileStart = DateTime.Now;
            await repository.InitializeAsync();
            await repository.UploadFirstFileAsync(csvFiles[0]);

            var firstFileDuration = (DateTime.Now - firstFileStart).TotalSeconds;
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] ✓ First file uploaded in {firstFileDuration:F1}s");
            Console.WriteLine(new string('=', 80));

            var remainingFiles = csvFiles.Skip(1).ToArray();
            var failedFiles = new List<string>();

            if (remainingFiles.Length > 0)
            {
                Console.WriteLine($"\n[{DateTime.Now:HH:mm:ss}] Starting parallel upload of {remainingFiles.Length} remaining files...");
                Console.WriteLine(new string('-', 80));

                var semaphore = new SemaphoreSlim(config.MaxWorkers);
                var tasks = remainingFiles.Select(async (file, index) =>
                {
                    await semaphore.WaitAsync();
                    try
                    {
                        return await repository.UploadFileAsync(file, index + 2, csvFiles.Length);
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }).ToArray();

                var results = await Task.WhenAll(tasks);
                failedFiles.AddRange(results.Where(r => !r.Success).Select(r => r.FileName));
            }

            // Display summary
            var (totalRows, successfulFiles) = repository.GetStats();

            Console.WriteLine("\n" + new string('=', 80));
            Console.WriteLine("UPLOAD SUMMARY");
            Console.WriteLine(new string('=', 80));
            var totalDuration = (DateTime.Now - startTime).TotalSeconds;
            Console.WriteLine($"Total time: {totalDuration:F1}s ({totalDuration / 60:F1} minutes)");
            Console.WriteLine($"Successful files: {successfulFiles}/{csvFiles.Length}");
            Console.WriteLine($"Total rows uploaded: {totalRows:N0}");

            if (failedFiles.Count > 0)
            {
                Console.WriteLine($"\nFailed files ({failedFiles.Count}):");
                foreach (var file in failedFiles)
                    Console.WriteLine($"  - {file}");
            }
            else
            {
                Console.WriteLine("\n✓ All files uploaded successfully!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nFATAL ERROR: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }
}
