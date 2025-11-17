using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

var config = new Config
{
    MaxWorkers = builder.Configuration.GetValue<int>("AppSettings:MaxWorkers"),
    MaxFiles = builder.Configuration.GetValue<int>("AppSettings:MaxFiles"),
    DataFolder = builder.Configuration.GetValue<string>("AppSettings:DataFolder") ?? "Food",
    BatchSize = builder.Configuration.GetValue<int>("AppSettings:BatchSize")
};

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
    options.UseNpgsql(connectionString);
});

builder.Services.AddSingleton(config);
builder.Services.AddTransient<Repository>();

var host = builder.Build();

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

    var tasks = csvFiles.Select(async (file, index) =>
    {
        await semaphore.WaitAsync();
        try
        {
            using var scope = host.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            var repository = scope.ServiceProvider.GetRequiredService<Repository>();
            var csvController = new CsvController(repository, config);

            return await csvController.ProcessFileAsync(file, index + 1, csvFiles.Length);
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
    Console.WriteLine($"Successful files: {results.Count(r => r.Success)}/{csvFiles.Length}");

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
