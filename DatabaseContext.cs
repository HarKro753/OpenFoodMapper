using Npgsql;

namespace OpenFood;

public class DatabaseContext
{
    private readonly Config _config;

    public DatabaseContext(Config config)
    {
        _config = config;
    }

    public async Task<NpgsqlConnection> OpenConnectionAsync()
    {
        var conn = new NpgsqlConnection(_config.GetConnectionString());
        await conn.OpenAsync();
        return conn;
    }

    public async Task CreateTableAsync()
    {
        await using var conn = await OpenConnectionAsync();
        var columnDefs = string.Join(", ", Product.Columns.Select(c => $"\"{c.Name}\" {c.Type}"));
        var command = $"DROP TABLE IF EXISTS products; CREATE TABLE products ({columnDefs}, PRIMARY KEY (\"code\"));";
        await using var cmd = new NpgsqlCommand(command, conn);
        await cmd.ExecuteNonQueryAsync();
    }
}
