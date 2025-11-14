namespace OpenFood.Database.Models;

public class Product
{
    public decimal Code { get; set; }
    public string? Url { get; set; }
    public string? ProductName { get; set; }
    public string? ProductBrand { get; set; }
    public string? ImageUrl { get; set; }

    // Scores
    public int? nutriScore { get; set; }
    public int? NovaGroup { get; set; }
    public decimal? EnvironmentalScore { get; set; }    

    // Ingredients
    public string? IngredientsTags { get; set; }
    public string? IngredientsText { get; set; }
    public string? AllergensEn { get; set; }
    public string? FoodGroupsEn { get; set; }
    public string? LabelsEn { get; set; }

    // Data Quality (use to filter for good results)
    public decimal? Completeness { get; set; } 
    public string? LastImageDatetime { get; set; }
    public string? LastModifiedDatetime { get; set; }

    // Ntrients pro 100g
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

    // Specific Fatty Acids
    public decimal? ButyricAcid100g { get; set; }
    public decimal? CaproicAcid100g { get; set; }
    public decimal? CaprylicAcid100g { get; set; }
    public decimal? CapricAcid100g { get; set; }
    public decimal? LauricAcid100g { get; set; }
    public decimal? MyristicAcid100g { get; set; }
    public decimal? PalmiticAcid100g { get; set; }
    public decimal? StearicAcid100g { get; set; }
    public decimal? ArachidicAcid100g { get; set; }
    public decimal? BehenicAcid100g { get; set; }
    public decimal? LignocericAcid100g { get; set; }
    public decimal? CeroticAcid100g { get; set; }
    public decimal? MontanicAcid100g { get; set; }
    public decimal? MelissicAcid100g { get; set; }
    public decimal? UnsaturatedFat100g { get; set; }
    public decimal? MonounsaturatedFat100g { get; set; }
    public decimal? Omega9Fat100g { get; set; }
    public decimal? PolyunsaturatedFat100g { get; set; }
    public decimal? Omega3Fat100g { get; set; }
    public decimal? Omega6Fat100g { get; set; }
    public decimal? AlphaLinolenicAcid100g { get; set; }
    public decimal? EicosapentaenoicAcid100g { get; set; }
    public decimal? DocosahexaenoicAcid100g { get; set; }
    public decimal? LinoleicAcid100g { get; set; }
    public decimal? ArachidonicAcid100g { get; set; }
    public decimal? GammaLinolenicAcid100g { get; set; }
    public decimal? DihomoGammaLinolenicAcid100g { get; set; }
    public decimal? OleicAcid100g { get; set; }
    public decimal? ElaidicAcid100g { get; set; }
    public decimal? GondoicAcid100g { get; set; }
    public decimal? MeadAcid100g { get; set; }
    public decimal? ErucicAcid100g { get; set; }
    public decimal? NervonicAcid100g { get; set; }

    // Specific Carbohydrates
    public decimal? Sucrose100g { get; set; }
    public decimal? Glucose100g { get; set; }
    public decimal? Fructose100g { get; set; }
    public decimal? Galactose100g { get; set; }
    public decimal? Lactose100g { get; set; }
    public decimal? Maltose100g { get; set; }
    public decimal? Maltodextrins100g { get; set; }
    public decimal? Psicose100g { get; set; }
    public decimal? Starch100g { get; set; }
    public decimal? Polyols100g { get; set; }
    public decimal? Erythritol100g { get; set; }
    public decimal? Isomalt100g { get; set; }
    public decimal? Maltitol100g { get; set; }
    public decimal? Sorbitol100g { get; set; }

    // Additional Fiber & Proteins
    public decimal? SolubleFiber100g { get; set; }
    public decimal? InsolubleFiber100g { get; set; }
    public decimal? Casein100g { get; set; }
    public decimal? SerumProteins100g { get; set; }
    public decimal? Nucleotides100g { get; set; }

    // Additional Vitamins
    public decimal? BetaCarotene100g { get; set; }
    public decimal? VitaminD100g { get; set; }
    public decimal? VitaminE100g { get; set; }
    public decimal? VitaminK100g { get; set; }
    public decimal? VitaminB1_100g { get; set; }
    public decimal? VitaminB2_100g { get; set; }
    public decimal? VitaminPp100g { get; set; }
    public decimal? VitaminB6_100g { get; set; }
    public decimal? VitaminB9_100g { get; set; }
    public decimal? Folates100g { get; set; }
    public decimal? VitaminB12_100g { get; set; }
    public decimal? Biotin100g { get; set; }
    public decimal? PantothenicAcid100g { get; set; }

    // Additional Minerals
    public decimal? Silica100g { get; set; }
    public decimal? Bicarbonate100g { get; set; }
    public decimal? Chloride100g { get; set; }
    public decimal? Phosphorus100g { get; set; }
    public decimal? Copper100g { get; set; }
    public decimal? Manganese100g { get; set; }
    public decimal? Fluoride100g { get; set; }
    public decimal? Selenium100g { get; set; }
    public decimal? Chromium100g { get; set; }
    public decimal? Molybdenum100g { get; set; }
    public decimal? Iodine100g { get; set; }

    // Other Nutrients
    public decimal? Caffeine100g { get; set; }
    public decimal? Taurine100g { get; set; }
    public decimal? Methylsulfonylmethane100g { get; set; }
    public decimal? Ph100g { get; set; }

    // Fruits/Vegetables
    public decimal? FruitsVegetablesNuts100g { get; set; }
    public decimal? FruitsVegetablesNutsDried100g { get; set; }
    public decimal? FruitsVegetablesNutsEstimate100g { get; set; }
    public decimal? FruitsVegetablesNutsEstimateFromIngredients100g { get; set; }


    // Navigation properties
    public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
    public ICollection<ProductAdditive> ProductAdditives { get; set; } = new List<ProductAdditive>();
    public ICollection<ProductIngredient> ProductIngredients { get; set; } = new List<ProductIngredient>();
    public ICollection<ProductCountry> ProductCountries { get; set; } = new List<ProductCountry>();
}
