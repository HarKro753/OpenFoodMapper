namespace OpenFood;

public class Config
{
    public int BatchSize { get; set; }
}

public class R2Config
{
    public string ApiBaseUri { get; set; } = string.Empty;
    public string AccountId { get; set; } = string.Empty;
    public string ApiToken { get; set; } = string.Empty;
    public string BucketName { get; set; } = string.Empty;
}
