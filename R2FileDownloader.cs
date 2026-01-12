namespace OpenFood;

using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;

public class R2FileDownloader
{
    private readonly R2Config _config;
    private readonly AmazonS3Client _s3Client;

    public R2FileDownloader(R2Config config)
    {
        _config = config;

        var credentials = new BasicAWSCredentials(_config.AccessKeyId, _config.SecretAccessKey);
        var s3Config = new AmazonS3Config
        {
            ServiceURL = _config.ServiceUrl,
            ForcePathStyle = true
        };

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
            await response.WriteResponseStreamToFileAsync(localPath, false, CancellationToken.None);

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

        return $"part_{(char)('a' + firstChar)}{(char)('a' + secondChar)}.csv";
    }
}
