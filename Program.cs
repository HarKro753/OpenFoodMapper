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

    var accessKey = builder.Configuration["Minio:AccessKey"] 
        ?? throw new InvalidOperationException("Configuration 'Minio:AccessKey' is missing.");
    var secretKey = builder.Configuration["Minio:SecretKey"] 
        ?? throw new InvalidOperationException("Configuration 'Minio:SecretKey' is missing.");
    var endpoint = builder.Configuration["Minio:Endpoint"] 
        ?? throw new InvalidOperationException("Configuration 'Minio:Endpoint' is missing.");
    var bucketName = builder.Configuration["Minio:BucketName"] 
        ?? throw new InvalidOperationException("Configuration 'Minio:BucketName' is missing.");

    var minioConfig = new MinioConfig
    {
        AccessKey = accessKey,
        SecretKey = secretKey,
        Endpoint = endpoint,
        BucketName = bucketName
    };

    var graphQLEndpoint = builder.Configuration["GraphQL:Endpoint"]
        ?? throw new InvalidOperationException("Configuration 'GraphQl:Endpoint' is missing.");

    builder.Services.AddSingleton(config);
    builder.Services.AddSingleton(new OpenFood.GraphQL.GraphQLService(graphQLEndpoint));

    var host = builder.Build();

    Log.Information("Starting Process");
    string? indexStr = Environment.GetEnvironmentVariable("JOB_COMPLETION_INDEX");

    if (string.IsNullOrEmpty(indexStr) || !int.TryParse(indexStr, out int index))
    {
        Log.Error("No valid JOB_COMPLETION_INDEX found");
        return;
    }

    string fileName = MinioFileDownloader.IndexToFileName(index);
    string localPath = Path.Combine(Path.GetTempPath(), fileName);

    var downloader = new MinioFileDownloader(minioConfig);
    string? downloadedFile = await downloader.DownloadFileAsync(fileName, localPath);

    if (downloadedFile == null)
    {
        Log.Error("File {FileName} not found in Minio", fileName);
        return;
    }

    using var scope = host.Services.CreateScope();
    var graphQLService = scope.ServiceProvider.GetRequiredService<OpenFood.GraphQL.GraphQLService>();
    var csvController = new CsvController(graphQLService, config);

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
