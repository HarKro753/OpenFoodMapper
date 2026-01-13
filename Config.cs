namespace OpenFood;

public class Config
{
    public int BatchSize { get; set; }
}

public class MinioConfig
{
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
    public string BucketName { get; set; } = "openfood";
}
