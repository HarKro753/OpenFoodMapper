using Npgsql;
using OpenFood.Models;

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

        await using (var cmd = new NpgsqlCommand("DROP TABLE IF EXISTS product_additives CASCADE; DROP TABLE IF EXISTS additives CASCADE; DROP TABLE IF EXISTS product_categories CASCADE; DROP TABLE IF EXISTS categories CASCADE; DROP TABLE IF EXISTS products CASCADE;", conn))
        {
            await cmd.ExecuteNonQueryAsync();
        }

        var excludedColumns = new[] { "categories", "categories_tags", "categories_en" };
        var productColumns = Product.Columns.Where(c => !excludedColumns.Contains(c.Name));
        var columnDefs = string.Join(", ", productColumns.Select(c => $"\"{c.Name}\" {c.Type}"));
        await using (var cmd = new NpgsqlCommand($"CREATE TABLE products ({columnDefs}, PRIMARY KEY (\"code\"));", conn))
        {
            await cmd.ExecuteNonQueryAsync();
        }

        var categoryColumnDefs = string.Join(", ", Category.Columns.Select(c => $"\"{c.Name}\" {c.Type}"));
        await using (var cmd = new NpgsqlCommand($"CREATE TABLE categories ({categoryColumnDefs});", conn))
        {
            await cmd.ExecuteNonQueryAsync();
        }

        var additiveColumnDefs = string.Join(", ", Additive.Columns.Select(c => $"\"{c.Name}\" {c.Type}"));
        await using (var cmd = new NpgsqlCommand($"CREATE TABLE additives ({additiveColumnDefs});", conn))
        {
            await cmd.ExecuteNonQueryAsync();
        }

        var productCategoryColumnDefs = string.Join(", ", ProductCategory.Columns.Select(c => $"\"{c.Name}\" {c.Type}"));
        await using (var cmd = new NpgsqlCommand($"CREATE TABLE product_categories ({productCategoryColumnDefs}, PRIMARY KEY (\"product_code\", \"category_id\"), FOREIGN KEY (\"product_code\") REFERENCES products(\"code\") ON DELETE CASCADE, FOREIGN KEY (\"category_id\") REFERENCES categories(\"id\") ON DELETE CASCADE);", conn))
        {
            await cmd.ExecuteNonQueryAsync();
        }

        var productAdditiveColumnDefs = string.Join(", ", ProductAdditive.Columns.Select(c => $"\"{c.Name}\" {c.Type}"));
        await using (var cmd = new NpgsqlCommand($"CREATE TABLE product_additives ({productAdditiveColumnDefs}, PRIMARY KEY (\"product_code\", \"additive_id\"), FOREIGN KEY (\"product_code\") REFERENCES products(\"code\") ON DELETE CASCADE, FOREIGN KEY (\"additive_id\") REFERENCES additives(\"id\") ON DELETE CASCADE);", conn))
        {
            await cmd.ExecuteNonQueryAsync();
        }

        await using (var cmd = new NpgsqlCommand("CREATE INDEX idx_product_categories_category ON product_categories(\"category_id\");", conn))
        {
            await cmd.ExecuteNonQueryAsync();
        }

        await using (var cmd = new NpgsqlCommand("CREATE INDEX idx_product_additives_additive ON product_additives(\"additive_id\");", conn))
        {
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
