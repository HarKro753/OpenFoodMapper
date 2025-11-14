namespace OpenFood;

public class Config
{
    public int MaxWorkers { get; set; } = 16;
    public int MaxFiles { get; set; } = 0;
    public string DataFolder { get; set; } = "Food";
    public int BatchSize { get; set; } = 1000;

    public string ConnectionString { get; set; } =
        "Host=192.168.178.186;Port=5432;Database=testdb;Username=myuser;Password=1234";
}
