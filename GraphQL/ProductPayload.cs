namespace OpenFood.GraphQL;

// UpsertProducts
public record UpsertProductsInput(
    List<ProductInput> Products
);

public record ProductInput(
    decimal Code,
    string? Url,
    string? ProductName,
    string? ProductBrand,
    string? ImageUrl,
    int? NutriScore,
    int? NovaGroup,
    decimal? EnvironmentalScore,
    decimal? Completeness,
    string? LastImageDatetime,
    string? LastModifiedDatetime,
    NutrientsInput? Nutrients
);

public record NutrientsInput(
    // Basic Nutrients
    decimal? EnergyKcal100g,
    decimal? EnergyFromFat100g,
    decimal? Fat100g,
    decimal? SaturatedFat100g,
    decimal? TransFat100g,
    decimal? Cholesterol100g,
    decimal? Carbohydrates100g,
    decimal? Sugars100g,
    decimal? AddedSugars100g,
    decimal? Fiber100g,
    decimal? Proteins100g,
    decimal? Salt100g,
    decimal? Sodium100g,
    decimal? Alcohol100g,
    decimal? VitaminA100g,
    decimal? VitaminC100g,
    decimal? Calcium100g,
    decimal? Iron100g,
    decimal? Magnesium100g,
    decimal? Zinc100g,
    decimal? Potassium100g,

    // Specific Fatty Acids
    decimal? ButyricAcid100g,
    decimal? CaproicAcid100g,
    decimal? CaprylicAcid100g,
    decimal? CapricAcid100g,
    decimal? LauricAcid100g,
    decimal? MyristicAcid100g,
    decimal? PalmiticAcid100g,
    decimal? StearicAcid100g,
    decimal? ArachidicAcid100g,
    decimal? BehenicAcid100g,
    decimal? LignocericAcid100g,
    decimal? CeroticAcid100g,
    decimal? MontanicAcid100g,
    decimal? MelissicAcid100g,
    decimal? UnsaturatedFat100g,
    decimal? MonounsaturatedFat100g,
    decimal? Omega9Fat100g,
    decimal? PolyunsaturatedFat100g,
    decimal? Omega3Fat100g,
    decimal? Omega6Fat100g,
    decimal? AlphaLinolenicAcid100g,
    decimal? EicosapentaenoicAcid100g,
    decimal? DocosahexaenoicAcid100g,
    decimal? LinoleicAcid100g,
    decimal? ArachidonicAcid100g,
    decimal? GammaLinolenicAcid100g,
    decimal? DihomoGammaLinolenicAcid100g,
    decimal? OleicAcid100g,
    decimal? ElaidicAcid100g,
    decimal? GondoicAcid100g,
    decimal? MeadAcid100g,
    decimal? ErucicAcid100g,
    decimal? NervonicAcid100g,

    // Specific Carbohydrates
    decimal? Sucrose100g,
    decimal? Glucose100g,
    decimal? Fructose100g,
    decimal? Galactose100g,
    decimal? Lactose100g,
    decimal? Maltose100g,
    decimal? Maltodextrins100g,
    decimal? Psicose100g,
    decimal? Starch100g,
    decimal? Polyols100g,
    decimal? Erythritol100g,
    decimal? Isomalt100g,
    decimal? Maltitol100g,
    decimal? Sorbitol100g,

    // Additional Fiber & Proteins
    decimal? SolubleFiber100g,
    decimal? InsolubleFiber100g,
    decimal? Casein100g,
    decimal? SerumProteins100g,
    decimal? Nucleotides100g,

    // Additional Vitamins
    decimal? BetaCarotene100g,
    decimal? VitaminD100g,
    decimal? VitaminE100g,
    decimal? VitaminK100g,
    decimal? VitaminB1_100g,
    decimal? VitaminB2_100g,
    decimal? VitaminPp100g,
    decimal? VitaminB6_100g,
    decimal? VitaminB9_100g,
    decimal? Folates100g,
    decimal? VitaminB12_100g,
    decimal? Biotin100g,
    decimal? PantothenicAcid100g,

    // Additional Minerals
    decimal? Silica100g,
    decimal? Bicarbonate100g,
    decimal? Chloride100g,
    decimal? Phosphorus100g,
    decimal? Copper100g,
    decimal? Manganese100g,
    decimal? Fluoride100g,
    decimal? Selenium100g,
    decimal? Chromium100g,
    decimal? Molybdenum100g,
    decimal? Iodine100g,

    // Other Nutrients
    decimal? Caffeine100g,
    decimal? Taurine100g,
    decimal? Methylsulfonylmethane100g,
    decimal? Ph100g,

    // Fruits/Vegetables
    decimal? FruitsVegetablesNutsEstimateFromIngredients100g
);

public record UpsertProductsPayload(
    int ProductsUpserted,
    string Message
);

public record UpsertProductsResponse(
    UpsertProductsPayload UpsertProducts
);

// LinkProductCategories
public record LinkProductCategoriesInput(
    decimal ProductCode,
    List<string> CategoryNames
);

public record LinkProductCategoriesPayload(
    bool Success
);

public record LinkProductCategoriesResponse(
    LinkProductCategoriesPayload LinkProductCategories
);

// LinkProductAdditives
public record LinkProductAdditivesInput(
    decimal ProductCode,
    List<string> AdditiveNames
);

public record LinkProductAdditivesPayload(
    bool Success
);

public record LinkProductAdditivesResponse(
    LinkProductAdditivesPayload LinkProductAdditives
);

// LinkProductIngredients
public record LinkProductIngredientsInput(
    decimal ProductCode,
    List<string> IngredientNames
);

public record LinkProductIngredientsPayload(
    bool Success
);

public record LinkProductIngredientsResponse(
    LinkProductIngredientsPayload LinkProductIngredients
);

// LinkProductCountries
public record LinkProductCountriesInput(
    decimal ProductCode,
    List<string> CountryNames
);

public record LinkProductCountriesPayload(
    bool Success
);

public record LinkProductCountriesResponse(
    LinkProductCountriesPayload LinkProductCountries
);

// LinkProductAllergens
public record LinkProductAllergensInput(
    decimal ProductCode,
    List<string> AllergenNames
);

public record LinkProductAllergensPayload(
    bool Success
);

public record LinkProductAllergensResponse(
    LinkProductAllergensPayload LinkProductAllergens
);

// LinkProductFoodGroups
public record LinkProductFoodGroupsInput(
    decimal ProductCode,
    List<string> FoodGroupNames
);

public record LinkProductFoodGroupsPayload(
    bool Success
);

public record LinkProductFoodGroupsResponse(
    LinkProductFoodGroupsPayload LinkProductFoodGroups
);

// LinkProductLabels
public record LinkProductLabelsInput(
    decimal ProductCode,
    List<string> LabelNames
);

public record LinkProductLabelsPayload(
    bool Success
);

public record LinkProductLabelsResponse(
    LinkProductLabelsPayload LinkProductLabels
);
