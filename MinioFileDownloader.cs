namespace OpenFood;

public class MinioFileDownloader
{
    private readonly MinioConfig _config;
    private readonly IAmazonS3 _s3Client;

    public MinioFileDownloader(MinioConfig config)
    {
        _config = config;
        var s3Config = new AmazonS3Config
        {
            ServiceURL = _config.Endpoint,
            ForcePathStyle = true 
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
            
            // Ensure directory exists
            var directory = Path.GetDirectoryName(localPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            await response.WriteResponseStreamToFileAsync(localPath, false, CancellationToken.None);
            return localPath;
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error downloading file {FileName} from Minio", fileName);
            throw;
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
}
