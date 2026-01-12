Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console(new CompactJsonFormatter())
    .CreateLogger();

Log.Information("Application wrapper started");
Console.WriteLine("Native Console: App Starting");

try
{
    var builder = Host.CreateApplicationBuilder(args);

    builder.Services.AddSerilog();

    var config = new Config
    {
        BatchSize = builder.Configuration.GetValue<int>("AppSettings:BatchSize")
    };

    var connectionString = builder.Configuration["AzureStorage:ConnectionString"] 
        ?? throw new InvalidOperationException("Configuration 'AzureStorage:ConnectionString' is missing.");
    var containerName = builder.Configuration["AzureStorage:ContainerName"] 
        ?? throw new InvalidOperationException("Configuration 'AzureStorage:ContainerName' is missing.");

    var azureConfig = new AzureConfig
    {
        ConnectionString = connectionString,
        ContainerName = containerName
    };

    builder.Services.AddDbContext<DatabaseContext>(options =>
    {
        var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
        options.UseNpgsql(connectionString);
    });

    builder.Services.AddSingleton(config);
    builder.Services.AddTransient<Repository>();

    var host = builder.Build();

    Log.Information("Starting Process");
    string? indexStr = Environment.GetEnvironmentVariable("JOB_COMPLETION_INDEX");

    if (string.IsNullOrEmpty(indexStr) || !int.TryParse(indexStr, out int index))
    {
        Log.Error("No valid JOB_COMPLETION_INDEX found");
        return;
    }

    string fileName = AzureFileDownloader.IndexToFileName(index);
    string localPath = Path.Combine(Path.GetTempPath(), fileName);

    var downloader = new AzureFileDownloader(azureConfig);
    string? downloadedFile = await downloader.DownloadFileAsync(fileName, localPath);

    if (downloadedFile == null)
    {
        Log.Error("File {FileName} not found in R2", fileName);
        return;
    }

    using var scope = host.Services.CreateScope();
    var repository = scope.ServiceProvider.GetRequiredService<Repository>();
    var csvController = new CsvController(repository, config);

    await csvController.ProcessFileAsync(downloadedFile);

    if (File.Exists(localPath))
    {
        File.Delete(localPath);
    }

    Log.Information("Processing completed");
}
catch (Exception ex)
{
    Log.Fatal(ex, "An error occurred during processing");
}
finally
{
    await Log.CloseAndFlushAsync();
}
