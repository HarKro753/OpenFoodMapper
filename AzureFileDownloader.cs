using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace OpenFood;

public class AzureFileDownloader
{
    private readonly AzureConfig _config;
    private readonly BlobServiceClient _blobServiceClient;
    private readonly BlobContainerClient _containerClient;

    public AzureFileDownloader(AzureConfig config)
    {
        _config = config;
        _blobServiceClient = new BlobServiceClient(_config.ConnectionString);
        _containerClient = _blobServiceClient.GetBlobContainerClient(_config.ContainerName);
    }

    public async Task<string?> DownloadFileAsync(string fileName, string localPath)
    {
        var blobClient = _containerClient.GetBlobClient(fileName);
        
        if (!await blobClient.ExistsAsync())
        {
            return null;
        }

        // Ensure directory exists
        var directory = Path.GetDirectoryName(localPath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await blobClient.DownloadToAsync(localPath);
        return localPath;
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
