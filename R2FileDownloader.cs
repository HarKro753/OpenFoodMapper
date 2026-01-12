namespace OpenFood;

public class R2FileDownloader
{
    private readonly R2Config _config;
    private readonly AmazonS3Client _s3Client;

    public R2FileDownloader(R2Config config)
    {
        _config = config;

        var s3Config = new AmazonS3Config
        {
            ServiceURL = _config.ApiBaseUri,
            ForcePathStyle = true, // R2 often requires path style access or specific domain config
            HttpClientFactory = new CustomHttpClientFactory()
        };

        var credentials = new BasicAWSCredentials(_config.AccessKey, _config.SecretKey);
        _s3Client = new AmazonS3Client(credentials, s3Config);
    }

    public async Task<string?> DownloadFileAsync(string fileName, string localPath)
    {
        try
        {
            var request = new GetObjectRequest
            {
                BucketName = _config.BucketName,
                Key = fileName
            };

            using var response = await _s3Client.GetObjectAsync(request);
            
            using var fileStream = new FileStream(localPath, FileMode.Create, FileAccess.Write, FileShare.None);
            await response.ResponseStream.CopyToAsync(fileStream);

            return localPath;
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
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

        return $"part_{(char)('a' + firstChar)}{(char)('a' + secondChar)}";
    }

    private class CustomHttpClientFactory : Amazon.Runtime.HttpClientFactory
    {
        public override HttpClient CreateHttpClient(IClientConfig clientConfig)
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            var client = new HttpClient(handler);
            return client;
        }
    }
}
