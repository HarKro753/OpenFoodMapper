namespace OpenFood.Models;

public class ProductAdditive
{
    public static readonly (string Name, string Type)[] Columns = {
        ("product_code", "NUMERIC NOT NULL"),
        ("additive_id", "INTEGER NOT NULL")
    };

    public static string[] ColumnNames => Columns.Select(c => c.Name).ToArray();
}
