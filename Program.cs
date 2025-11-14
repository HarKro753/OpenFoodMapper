using OpenFood;
using OpenFood.Database.Models;

var startTime = DateTime.Now;

var config = new Config();

using (var initDbContext = new DatabaseContext(config))
{
    await initDbContext.CreateTableAsync();
}

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

    var semaphore = new SemaphoreSlim(config.MaxWorkers);
    var statsLock = new object();
    var totalRows = 0;
    var successfulFiles = 0;

    var tasks = csvFiles.Select(async (file, index) =>
    {
        await semaphore.WaitAsync();
        try
        {
            // Create a new DbContext for each parallel task
            using var dbContext = new DatabaseContext(config);
            var repository = new Repository(dbContext);
            var csvController = new CsvController(repository, config);

            var result = await csvController.ProcessFileAsync(file, index + 1, csvFiles.Length);

            // Update stats in a thread-safe manner
            var stats = csvController.GetStats();
            lock (statsLock)
            {
                totalRows += stats.TotalRows;
                if (result.Success)
                    successfulFiles++;
            }

            return result;
        }
        finally
        {
            semaphore.Release();
        }
    }).ToArray();

    var results = await Task.WhenAll(tasks);
    var failedFiles = results.Where(r => !r.Success).Select(r => r.FileName).ToList();

    Console.WriteLine("\n" + new string('=', 80));
    Console.WriteLine("UPLOAD SUMMARY");
    Console.WriteLine(new string('=', 80));
    var totalDuration = (DateTime.Now - startTime).TotalSeconds;
    Console.WriteLine($"Total time: {totalDuration:F1}s ({totalDuration / 60:F1} minutes)");
    lock (statsLock)
    {
        Console.WriteLine($"Successful files: {successfulFiles}/{csvFiles.Length}");
        Console.WriteLine($"Total rows uploaded: {totalRows:N0}");
    }

    if (failedFiles.Count > 0)
    {
        Console.WriteLine($"\nFailed files ({failedFiles.Count}):");
        foreach (var file in failedFiles)
            Console.WriteLine($"  - {file}");
    }
    else
    {
        Console.WriteLine("\nAll files uploaded successfully!");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"\nFATAL ERROR: {ex.Message}");
    Console.WriteLine(ex.StackTrace);
}
