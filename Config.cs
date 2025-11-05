namespace OpenFood;

public class Config
{
    public string DbHost { get; set; } = "192.168.178.186";
    public string DbPort { get; set; } = "5432";
    public string DbName { get; set; } = "mydb";
    public string DbUser { get; set; } = "myuser";
    public string DbPassword { get; set; } = "1234";
    public int MaxWorkers { get; set; } = 16;
    public string DataFolder { get; set; } = "Food";

    public string GetConnectionString() =>
        $"Host={DbHost};Port={DbPort};Database={DbName};Username={DbUser};Password={DbPassword}";
}
