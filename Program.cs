// Bypass SSL certificate validation globally
System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Error()
    .WriteTo.Console(new CompactJsonFormatter())
    .CreateLogger();

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSerilog();

var config = new Config
{
    BatchSize = builder.Configuration.GetValue<int>("AppSettings:BatchSize")
};

var r2Config = new R2Config
{
    ApiBaseUri = builder.Configuration["R2:ApiBaseUri"] ?? string.Empty,
    AccountId = builder.Configuration["R2:AccountId"] ?? string.Empty,
    ApiToken = builder.Configuration["R2:ApiToken"] ?? string.Empty,
    BucketName = builder.Configuration["R2:BucketName"] ?? string.Empty
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

try
{
    string? indexStr = Environment.GetEnvironmentVariable("JOB_COMPLETION_INDEX");

    if (string.IsNullOrEmpty(indexStr) || !int.TryParse(indexStr, out int index))
    {
        Log.Error("No valid JOB_COMPLETION_INDEX found");
        return;
    }

    string fileName = R2FileDownloader.IndexToFileName(index);
    string localPath = Path.Combine(Path.GetTempPath(), fileName);

    var downloader = new R2FileDownloader(r2Config);
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
