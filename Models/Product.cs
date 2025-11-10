namespace OpenFood.Models;

public class Product
{
    public static readonly (string Name, string Type)[] Columns = {
        // Primary Key
        ("code", "NUMERIC"),

        // Basic info
        ("url", "TEXT"),
        ("product_name", "TEXT"),
        ("brands", "TEXT"),
        ("image_url", "TEXT"),

        // Scores
        ("nutriscore_score", "INTEGER"),
        ("nova_group", "INTEGER"),
        ("environmental_score_score", "NUMERIC"),

        // Data Quality
        ("completeness", "NUMERIC"),
        ("last_image_datetime", "TEXT"),
        ("last_modified_datetime", "TEXT"),

        // Ingredients
        ("additives_en", "TEXT"),
        ("ingredients_tags", "TEXT"),

        // Nutrients per 100g
        ("energy-kcal_100g", "NUMERIC"),
        ("energy-from-fat_100g", "NUMERIC"),
        ("fat_100g", "NUMERIC"),
        ("saturated-fat_100g", "NUMERIC"),
        ("trans-fat_100g", "NUMERIC"),
        ("cholesterol_100g", "NUMERIC"),
        ("carbohydrates_100g", "NUMERIC"),
        ("sugars_100g", "NUMERIC"),
        ("added-sugars_100g", "NUMERIC"),
        ("fiber_100g", "NUMERIC"),
        ("proteins_100g", "NUMERIC"),
        ("salt_100g", "NUMERIC"),
        ("sodium_100g", "NUMERIC"),
        ("alcohol_100g", "NUMERIC"),
        ("vitamin-a_100g", "NUMERIC"),
        ("vitamin-c_100g", "NUMERIC"),
        ("calcium_100g", "NUMERIC"),
        ("iron_100g", "NUMERIC"),
        ("magnesium_100g", "NUMERIC"),
        ("zinc_100g", "NUMERIC"),
        ("potassium_100g", "NUMERIC")
    };

    public static string[] ColumnNames => Columns.Select(c => c.Name).ToArray();
}
