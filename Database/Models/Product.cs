namespace OpenFood.Database.Models;

public class Product
{
    public decimal Code { get; set; }
    public string? Url { get; set; }
    public string? ProductName { get; set; }
    public string? Brands { get; set; }
    public string? ImageUrl { get; set; }
    public int? NutriscoreScore { get; set; }
    public int? NovaGroup { get; set; }
    public decimal? EnvironmentalScoreScore { get; set; }
    public decimal? Completeness { get; set; }
    public string? LastImageDatetime { get; set; }
    public string? LastModifiedDatetime { get; set; }
    public string? AdditivesEn { get; set; }
    public string? IngredientsTags { get; set; }
    public decimal? EnergyKcal100g { get; set; }
    public decimal? EnergyFromFat100g { get; set; }
    public decimal? Fat100g { get; set; }
    public decimal? SaturatedFat100g { get; set; }
    public decimal? TransFat100g { get; set; }
    public decimal? Cholesterol100g { get; set; }
    public decimal? Carbohydrates100g { get; set; }
    public decimal? Sugars100g { get; set; }
    public decimal? AddedSugars100g { get; set; }
    public decimal? Fiber100g { get; set; }
    public decimal? Proteins100g { get; set; }
    public decimal? Salt100g { get; set; }
    public decimal? Sodium100g { get; set; }
    public decimal? Alcohol100g { get; set; }
    public decimal? VitaminA100g { get; set; }
    public decimal? VitaminC100g { get; set; }
    public decimal? Calcium100g { get; set; }
    public decimal? Iron100g { get; set; }
    public decimal? Magnesium100g { get; set; }
    public decimal? Zinc100g { get; set; }
    public decimal? Potassium100g { get; set; }
    public string? CategoriesEn { get; set; }

    public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
}
