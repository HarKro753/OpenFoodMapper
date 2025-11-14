namespace OpenFood;

public class Config
{
    public int MaxWorkers { get; set; } = 16;

    public string ConnectionString { get; set; } =
        $"Host=192.168.178.186;Port=5432;Database=testdb;Username=myuser;Password=1234";
}
