namespace OpenFood;

public class R2FileDownloader
{
    private readonly R2Config _config;
    private readonly CloudflareR2Client _r2Client;

    public R2FileDownloader(R2Config config)
    {
        _config = config;

        var options = Options.Create(new CloudflareR2Options
        {
            ApiBaseUri = _config.ApiBaseUri,
            AccountId = _config.AccountId,
            ApiToken = _config.ApiToken
        });

        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        };
        var httpClient = new HttpClient(handler);
        _r2Client = new CloudflareR2Client(httpClient, options);
    }

    public async Task<string?> DownloadFileAsync(string fileName, string localPath)
    {
        try
        {
            var blobPath = $"{_config.BucketName}/{fileName}";
            using var blobStream = await _r2Client.GetBlobAsync(blobPath, CancellationToken.None);

            using var fileStream = new FileStream(localPath, FileMode.Create, FileAccess.Write, FileShare.None);
            await blobStream.CopyToAsync(fileStream);

            return localPath;
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public static string IndexToFileName(int index)
    {
        if (index < 0 || index > 675)
            throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 0 and 675 (part_aa to part_zz)");

        int firstChar = index / 26;
        int secondChar = index % 26;

        return $"part_{(char)('a' + firstChar)}{(char)('a' + secondChar)}.csv";
    }
}
