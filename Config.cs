namespace OpenFood;

public class Config
{
    public int BatchSize { get; set; }
}

public class R2Config
{
    public string AccessKeyId { get; set; } = string.Empty;
    public string SecretAccessKey { get; set; } = string.Empty;
    public string BucketName { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string ServiceUrl { get; set; } = string.Empty;
}
