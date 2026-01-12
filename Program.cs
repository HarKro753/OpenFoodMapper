var builder = Host.CreateApplicationBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddFilter((category, level) => level >= LogLevel.Error);

var config = new Config
{
    BatchSize = builder.Configuration.GetValue<int>("AppSettings:BatchSize")
};

var r2Config = new R2Config
{
    AccessKeyId = builder.Configuration["R2:AccessKeyId"] ?? string.Empty,
    SecretAccessKey = builder.Configuration["R2:SecretAccessKey"] ?? string.Empty,
    BucketName = builder.Configuration["R2:BucketName"] ?? string.Empty,
    Token = builder.Configuration["R2:Token"] ?? string.Empty,
    ServiceUrl = builder.Configuration["R2:ServiceUrl"] ?? string.Empty
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
    string? indexStr = Environment.GetEnvironmentVariable("JOB_COMPLETION_INDEX");

    if (string.IsNullOrEmpty(indexStr) || !int.TryParse(indexStr, out int index))
    {
        Console.WriteLine("No valid JOB_COMPLETION_INDEX found");
        return;
    }

    string fileName = R2FileDownloader.IndexToFileName(index);
    string localPath = Path.Combine(Path.GetTempPath(), fileName);

    var downloader = new R2FileDownloader(r2Config);
    string? downloadedFile = await downloader.DownloadFileAsync(fileName, localPath);

    if (downloadedFile == null)
    {
        Console.WriteLine($"File {fileName} not found in R2");
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

    Console.WriteLine("Processing completed");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
