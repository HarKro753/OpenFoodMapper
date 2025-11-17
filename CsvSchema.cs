namespace OpenFood;

public static class CsvSchema
{
    public enum Column
    {
        // Basic Product Information - string
        Code = 0,                           // decimal (primary key)
        Url = 1,                            // string
        ProductName = 10,                   // string
        Brands = 18,                        // string
        ImageUrl = 82,                      // string

        // Metadata - string/int
        Creator = 2,                        // string
        CreatedT = 3,                       // int (timestamp)
        CreatedDatetime = 4,                // string
        LastModifiedT = 5,                  // int (timestamp)
        LastModifiedDatetime = 6,           // string
        LastModifiedBy = 7,                 // string
        LastUpdatedT = 8,                   // int (timestamp)
        LastUpdatedDatetime = 9,            // string
        Owner = 73,                         // string

        // Product Details - string
        AbbreviatedProductName = 11,        // string
        GenericName = 12,                   // string
        Quantity = 13,                      // string
        Packaging = 14,                     // string
        PackagingTags = 15,                 // string (comma-separated)
        PackagingEn = 16,                   // string
        PackagingText = 17,                 // string
        BrandsTags = 19,                    // string (comma-separated)
        BrandsEn = 20,                      // string
        BrandOwner = 68,                    // string
        ProductQuantity = 72,               // string

        // Categories (comma-separated lists) - string
        Categories = 21,                    // string (comma-separated)
        CategoriesTags = 22,                // string (comma-separated)
        CategoriesEn = 23,                  // string (comma-separated)
        MainCategory = 80,                  // string
        MainCategoryEn = 81,                // string

        // Origins & Manufacturing - string
        Origins = 24,                       // string (comma-separated)
        OriginsTags = 25,                   // string (comma-separated)
        OriginsEn = 26,                     // string (comma-separated)
        ManufacturingPlaces = 27,           // string (comma-separated)
        ManufacturingPlacesTags = 28,       // string (comma-separated)

        // Labels (comma-separated lists) - string
        Labels = 29,                        // string (comma-separated)
        LabelsTags = 30,                    // string (comma-separated)
        LabelsEn = 31,                      // string (comma-separated)

        // Location Data - string
        EmbCodes = 32,                      // string (comma-separated)
        EmbCodesTags = 33,                  // string (comma-separated)
        FirstPackagingCodeGeo = 34,         // string
        Cities = 35,                        // string (comma-separated)
        CitiesTags = 36,                    // string (comma-separated)
        PurchasePlaces = 37,                // string (comma-separated)
        Stores = 38,                        // string (comma-separated)
        Countries = 39,                     // string (comma-separated)
        CountriesTags = 40,                 // string (comma-separated)
        CountriesEn = 41,                   // string (comma-separated)

        // Ingredients - string
        IngredientsText = 42,               // string
        IngredientsTags = 43,               // string (comma-separated)
        IngredientsAnalysisTags = 44,       // string (comma-separated)

        // Allergens (comma-separated lists) - string
        Allergens = 45,                     // string (comma-separated)
        AllergensEn = 46,                   // string (comma-separated)
        Traces = 47,                        // string (comma-separated)
        TracesTags = 48,                    // string (comma-separated)
        TracesEn = 49,                      // string (comma-separated)

        // Additives (comma-separated lists) - string/int
        AdditivesN = 53,                    // int
        Additives = 54,                     // string (comma-separated)
        AdditivesTags = 55,                 // string (comma-separated)
        AdditivesEn = 56,                   // string (comma-separated)

        // Food Groups (comma-separated lists) - string
        FoodGroups = 62,                    // string (comma-separated)
        FoodGroupsTags = 63,                // string (comma-separated)
        FoodGroupsEn = 64,                  // string (comma-separated)

        // Serving Information - string/decimal
        ServingSize = 50,                   // string
        ServingQuantity = 51,               // decimal
        NoNutritionData = 52,               // string

        // Scores - int/decimal/string
        NutriscoreScore = 57,               // int
        NutriscoreGrade = 58,               // string
        NovaGroup = 59,                     // int
        PnnsGroups1 = 60,                   // string
        PnnsGroups2 = 61,                   // string
        EnvironmentalScoreScore = 69,       // decimal
        EnvironmentalScoreGrade = 70,       // string
        NutritionScoreFr100g = 201,         // decimal
        NutritionScoreUk100g = 202,         // decimal

        // Data Quality - string/decimal/int
        States = 65,                        // string (comma-separated)
        StatesTags = 66,                    // string (comma-separated)
        StatesEn = 67,                      // string (comma-separated)
        NutrientLevelsTags = 71,            // string (comma-separated)
        DataQualityErrorsTags = 74,         // string (comma-separated)
        UniqueScansN = 75,                  // int
        PopularityTags = 76,                // string (comma-separated)
        Completeness = 77,                  // decimal
        LastImageT = 78,                    // int (timestamp)
        LastImageDatetime = 79,             // string

        // Image URLs - string
        ImageSmallUrl = 83,                 // string
        ImageIngredientsUrl = 84,           // string
        ImageIngredientsSmallUrl = 85,      // string
        ImageNutritionUrl = 86,             // string
        ImageNutritionSmallUrl = 87,        // string

        // Basic Nutrients per 100g - decimal
        EnergyKj100g = 88,                  // decimal
        EnergyKcal100g = 89,                // decimal
        Energy100g = 90,                    // decimal
        EnergyFromFat100g = 91,             // decimal
        Fat100g = 92,                       // decimal
        SaturatedFat100g = 93,              // decimal
        TransFat100g = 127,                 // decimal
        Cholesterol100g = 128,              // decimal
        Carbohydrates100g = 129,            // decimal
        Sugars100g = 130,                   // decimal
        AddedSugars100g = 131,              // decimal
        Fiber100g = 146,                    // decimal
        Proteins100g = 149,                 // decimal
        Salt100g = 153,                     // decimal
        AddedSalt100g = 154,                // decimal
        Sodium100g = 155,                   // decimal
        Alcohol100g = 156,                  // decimal

        // Vitamins per 100g - decimal
        VitaminA100g = 157,                 // decimal
        BetaCarotene100g = 158,             // decimal
        VitaminD100g = 159,                 // decimal
        VitaminE100g = 160,                 // decimal
        VitaminK100g = 161,                 // decimal
        VitaminC100g = 162,                 // decimal
        VitaminB1_100g = 163,               // decimal
        VitaminB2_100g = 164,               // decimal
        VitaminPp100g = 165,                // decimal
        VitaminB6_100g = 166,               // decimal
        VitaminB9_100g = 167,               // decimal
        Folates100g = 168,                  // decimal
        VitaminB12_100g = 169,              // decimal
        Biotin100g = 170,                   // decimal
        PantothenicAcid100g = 171,          // decimal

        // Minerals per 100g - decimal
        Calcium100g = 176,                  // decimal
        Iron100g = 178,                     // decimal
        Magnesium100g = 179,                // decimal
        Zinc100g = 180,                     // decimal
        Potassium100g = 174,                // decimal
        Silica100g = 172,                   // decimal
        Bicarbonate100g = 173,              // decimal
        Chloride100g = 175,                 // decimal
        Phosphorus100g = 177,               // decimal
        Copper100g = 181,                   // decimal
        Manganese100g = 182,                // decimal
        Fluoride100g = 183,                 // decimal
        Selenium100g = 184,                 // decimal
        Chromium100g = 185,                 // decimal
        Molybdenum100g = 186,               // decimal
        Iodine100g = 187,                   // decimal

        // Specific Saturated Fatty Acids per 100g - decimal
        ButyricAcid100g = 94,               // decimal
        CaproicAcid100g = 95,               // decimal
        CaprylicAcid100g = 96,              // decimal
        CapricAcid100g = 97,                // decimal
        LauricAcid100g = 98,                // decimal
        MyristicAcid100g = 99,              // decimal
        PalmiticAcid100g = 100,             // decimal
        StearicAcid100g = 101,              // decimal
        ArachidicAcid100g = 102,            // decimal
        BehenicAcid100g = 103,              // decimal
        LignocericAcid100g = 104,           // decimal
        CeroticAcid100g = 105,              // decimal
        MontanicAcid100g = 106,             // decimal
        MelissicAcid100g = 107,             // decimal

        // Unsaturated Fats per 100g - decimal
        UnsaturatedFat100g = 108,           // decimal
        MonounsaturatedFat100g = 109,       // decimal
        Omega9Fat100g = 110,                // decimal
        PolyunsaturatedFat100g = 111,       // decimal
        Omega3Fat100g = 112,                // decimal
        Omega6Fat100g = 113,                // decimal

        // Specific Unsaturated Fatty Acids per 100g - decimal
        AlphaLinolenicAcid100g = 114,       // decimal
        EicosapentaenoicAcid100g = 115,     // decimal
        DocosahexaenoicAcid100g = 116,      // decimal
        LinoleicAcid100g = 117,             // decimal
        ArachidonicAcid100g = 118,          // decimal
        GammaLinolenicAcid100g = 119,       // decimal
        DihomoGammaLinolenicAcid100g = 120, // decimal
        OleicAcid100g = 121,                // decimal
        ElaidicAcid100g = 122,              // decimal
        GondoicAcid100g = 123,              // decimal
        MeadAcid100g = 124,                 // decimal
        ErucicAcid100g = 125,               // decimal
        NervonicAcid100g = 126,             // decimal

        // Specific Carbohydrates per 100g - decimal
        Sucrose100g = 132,                  // decimal
        Glucose100g = 133,                  // decimal
        Fructose100g = 134,                 // decimal
        Galactose100g = 135,                // decimal
        Lactose100g = 136,                  // decimal
        Maltose100g = 137,                  // decimal
        Maltodextrins100g = 138,            // decimal
        Psicose100g = 139,                  // decimal
        Starch100g = 140,                   // decimal
        Polyols100g = 141,                  // decimal
        Erythritol100g = 142,               // decimal
        Isomalt100g = 143,                  // decimal
        Maltitol100g = 144,                 // decimal
        Sorbitol100g = 145,                 // decimal
        CarbohydratesTotal100g = 213,       // decimal

        // Additional Fiber & Proteins per 100g - decimal
        SolubleFiber100g = 147,             // decimal
        InsolubleFiber100g = 148,           // decimal
        Casein100g = 150,                   // decimal
        SerumProteins100g = 151,            // decimal
        Nucleotides100g = 152,              // decimal

        // Other Nutrients per 100g - decimal
        Caffeine100g = 188,                 // decimal
        Taurine100g = 189,                  // decimal
        Methylsulfonylmethane100g = 190,    // decimal
        Ph100g = 191,                       // decimal
        GlycemicIndex100g = 203,            // decimal
        WaterHardness100g = 204,            // decimal
        Choline100g = 205,                  // decimal
        Phylloquinone100g = 206,            // decimal
        BetaGlucan100g = 207,               // decimal
        Inositol100g = 208,                 // decimal
        Carnitine100g = 209,                // decimal
        Sulphate100g = 210,                 // decimal
        Nitrate100g = 211,                  // decimal
        Acidity100g = 212,                  // decimal

        // Fruits/Vegetables per 100g - decimal
        FruitsVegetablesNuts100g = 192,                             // decimal
        FruitsVegetablesNutsDried100g = 193,                        // decimal
        FruitsVegetablesNutsEstimate100g = 194,                     // decimal
        FruitsVegetablesNutsEstimateFromIngredients100g = 195,      // decimal

        // Environmental & Quality Metrics - decimal
        CollagenMeatProteinRatio100g = 196,         // decimal
        Cocoa100g = 197,                            // decimal
        Chlorophyl100g = 198,                       // decimal
        CarbonFootprint100g = 199,                  // decimal
        CarbonFootprintFromMeatOrFish100g = 200     // decimal
    }

    public static string? Get(string[] fields, Column column)
    {
        var index = (int)column;
        if (index >= fields.Length) return null;
        var value = fields[index];
        return string.IsNullOrWhiteSpace(value) ? null : value;
    }

    public static decimal? GetDecimal(string[] fields, Column column)
    {
        var value = Get(fields, column);
        if (value == null) return null;
        return decimal.TryParse(value, out var result) ? result : null;
    }

    public static int? GetInt(string[] fields, Column column)
    {
        var value = Get(fields, column);
        if (value == null) return null;
        return int.TryParse(value, out var result) ? result : null;
    }

    public static List<string> GetList(string[] fields, Column column)
    {
        var value = Get(fields, column);
        return value == null ? [] : Tools.ParseCommaSeparatedValues(value);
    }
}
