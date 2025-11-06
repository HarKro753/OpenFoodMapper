namespace OpenFood.Models;

public class Category
{
    public static readonly (string Name, string Type)[] Columns = {
        ("id", "SERIAL PRIMARY KEY"),
        ("name", "TEXT NOT NULL"),
        ("parent_id", "INTEGER")
    };

    public static string[] ColumnNames => Columns.Select(c => c.Name).ToArray();
}
