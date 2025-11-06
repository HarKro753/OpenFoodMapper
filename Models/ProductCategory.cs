namespace OpenFood.Models;

public class ProductCategory
{
    public static readonly (string Name, string Type)[] Columns = {
        ("product_code", "BIGINT NOT NULL"),
        ("category_id", "INTEGER NOT NULL")
    };

    public static string[] ColumnNames => Columns.Select(c => c.Name).ToArray();
}
