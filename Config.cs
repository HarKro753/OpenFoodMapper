namespace OpenFood;

public class Config
{
    public int MaxWorkers { get; set; }
    public int MaxFiles { get; set; }
    public string DataFolder { get; set; } = string.Empty;
    public int BatchSize { get; set; }
}
