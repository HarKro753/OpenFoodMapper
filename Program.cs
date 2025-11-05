namespace OpenFood;

class Program
{
    static async Task Main()
    {
        var startTime = DateTime.Now;

        var config = new Config();
        var dbContext = new DatabaseContext(config);
        var repository = new Repository(dbContext, config);

        try
        {
            var allFiles = Directory.GetFiles(config.DataFolder, "part_*")
                .OrderBy(f => f)
                .ToArray();

            if (allFiles.Length == 0)
            {
                Console.WriteLine($"ERROR: No files found in {config.DataFolder}/ folder!");
                return;
            }

            var csvFiles = config.MaxFiles > 0
                ? allFiles.Take(config.MaxFiles).ToArray()
                : allFiles;

            await dbContext.CreateTableAsync();

            var failedFiles = new List<string>();
            var semaphore = new SemaphoreSlim(config.MaxWorkers);
            var tasks = csvFiles.Select(async (file, index) =>
            {
                await semaphore.WaitAsync();
                try
                {
                    return await repository.UploadFileAsync(file, index + 1, csvFiles.Length);
                }
                finally
                {
                    semaphore.Release();
                }
            }).ToArray();

            var results = await Task.WhenAll(tasks);
            failedFiles.AddRange(results.Where(r => !r.Success).Select(r => r.FileName));

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
