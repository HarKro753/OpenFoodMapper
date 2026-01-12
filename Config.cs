namespace OpenFood;

public class Config
{
    public int BatchSize { get; set; }
}

public class AzureConfig
{
    public string ConnectionString { get; set; } = string.Empty;
    public string ContainerName { get; set; } = string.Empty;
}
