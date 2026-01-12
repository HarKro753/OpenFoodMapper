namespace OpenFood;

public class CsvController
{
    private readonly Repository _repository;
    private readonly Config _config;

    public CsvController(Repository repository, Config config)
    {
        _repository = repository;
        _config = config;
    }

    public async Task ProcessFileAsync(string filePath)
    {
        var products = new List<Product>();
        var categoriesMap = new Dictionary<decimal, List<string>>();
        var additivesMap = new Dictionary<decimal, List<string>>();
        var ingredientsMap = new Dictionary<decimal, List<string>>();
        var countriesMap = new Dictionary<decimal, List<string>>();
        var allergensMap = new Dictionary<decimal, List<string>>();
        var foodGroupsMap = new Dictionary<decimal, List<string>>();
        var labelsMap = new Dictionary<decimal, List<string>>();

        await foreach (var line in File.ReadLinesAsync(filePath))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            var fields = ParseCsvLine(line);
            if (fields.Length < 10) continue;

            var codeStr = CsvSchema.Get(fields, CsvSchema.Column.Code);
            if (codeStr == null || !decimal.TryParse(codeStr, out var code)) continue;

            var product = MapProduct(fields, code);
            products.Add(product);

            var categories = CsvSchema.GetList(fields, CsvSchema.Column.CategoriesEn);
            if (categories.Count > 0) categoriesMap[code] = categories;

            var additives = CsvSchema.GetList(fields, CsvSchema.Column.AdditivesTags);
            if (additives.Count > 0) additivesMap[code] = additives;

            var ingredients = CsvSchema.GetList(fields, CsvSchema.Column.IngredientsTags);
            if (ingredients.Count > 0) ingredientsMap[code] = ingredients;

            var countries = CsvSchema.GetList(fields, CsvSchema.Column.CountriesEn);
            if (countries.Count > 0) countriesMap[code] = countries;

            var allergens = CsvSchema.GetList(fields, CsvSchema.Column.AllergensEn);
            if (allergens.Count > 0) allergensMap[code] = allergens;

            var foodGroups = CsvSchema.GetList(fields, CsvSchema.Column.FoodGroupsEn);
            if (foodGroups.Count > 0) foodGroupsMap[code] = foodGroups;

            var labels = CsvSchema.GetList(fields, CsvSchema.Column.LabelsEn);
            if (labels.Count > 0) labelsMap[code] = labels;

            if (products.Count >= _config.BatchSize)
            {
                await ProcessBatchAsync(products, categoriesMap, additivesMap, ingredientsMap, countriesMap, allergensMap, foodGroupsMap, labelsMap);
                products.Clear();
                categoriesMap.Clear();
                additivesMap.Clear();
                ingredientsMap.Clear();
                countriesMap.Clear();
                allergensMap.Clear();
                foodGroupsMap.Clear();
                labelsMap.Clear();
            }
        }

        if (products.Count > 0)
        {
            await ProcessBatchAsync(products, categoriesMap, additivesMap, ingredientsMap, countriesMap, allergensMap, foodGroupsMap, labelsMap);
        }
    }

    private async Task ProcessBatchAsync(
        List<Product> products,
        Dictionary<decimal, List<string>> categoriesMap,
        Dictionary<decimal, List<string>> additivesMap,
        Dictionary<decimal, List<string>> ingredientsMap,
        Dictionary<decimal, List<string>> countriesMap,
        Dictionary<decimal, List<string>> allergensMap,
        Dictionary<decimal, List<string>> foodGroupsMap,
        Dictionary<decimal, List<string>> labelsMap)
    {
        await _repository.UpsertProductsAsync(products);

        var allCategoryNames = categoriesMap.Values.SelectMany(x => x).Distinct().ToList();
        var categoryIdMap = await _repository.GetOrCreateCategoriesAsync(allCategoryNames);
        foreach (var (code, categoryNames) in categoriesMap)
        {
            var categoryIds = categoryNames
                .Where(name => categoryIdMap.ContainsKey(name))
                .Select(name => categoryIdMap[name])
                .ToList();
            await _repository.LinkProductCategoriesAsync(code, categoryIds);
        }

        var allAdditiveNames = additivesMap.Values.SelectMany(x => x).Distinct().ToList();
        var additiveIdMap = await _repository.GetOrCreateAdditivesAsync(allAdditiveNames);
        foreach (var (code, additiveNames) in additivesMap)
        {
            var additiveIds = additiveNames
                .Where(name => additiveIdMap.ContainsKey(name))
                .Select(name => additiveIdMap[name])
                .ToList();
            await _repository.LinkProductAdditivesAsync(code, additiveIds);
        }

        var allIngredientNames = ingredientsMap.Values.SelectMany(x => x).Distinct().ToList();
        var ingredientIdMap = await _repository.GetOrCreateIngredientsAsync(allIngredientNames);
        foreach (var (code, ingredientNames) in ingredientsMap)
        {
            var ingredientIds = ingredientNames
                .Where(name => ingredientIdMap.ContainsKey(name))
                .Select(name => ingredientIdMap[name])
                .ToList();
            await _repository.LinkProductIngredientsAsync(code, ingredientIds);
        }

        var allCountryNames = countriesMap.Values.SelectMany(x => x).Distinct().ToList();
        var countryIdMap = await _repository.GetOrCreateCountriesAsync(allCountryNames);
        foreach (var (code, countryNames) in countriesMap)
        {
            var countryIds = countryNames
                .Where(name => countryIdMap.ContainsKey(name))
                .Select(name => countryIdMap[name])
                .ToList();
            await _repository.LinkProductCountriesAsync(code, countryIds);
        }

        var allAllergenNames = allergensMap.Values.SelectMany(x => x).Distinct().ToList();
        var allergenIdMap = await _repository.GetOrCreateAllergensAsync(allAllergenNames);
        foreach (var (code, allergenNames) in allergensMap)
        {
            var allergenIds = allergenNames
                .Where(name => allergenIdMap.ContainsKey(name))
                .Select(name => allergenIdMap[name])
                .ToList();
            await _repository.LinkProductAllergensAsync(code, allergenIds);
        }

        var allFoodGroupNames = foodGroupsMap.Values.SelectMany(x => x).Distinct().ToList();
        var foodGroupIdMap = await _repository.GetOrCreateFoodGroupsAsync(allFoodGroupNames);
        foreach (var (code, foodGroupNames) in foodGroupsMap)
        {
            var foodGroupIds = foodGroupNames
                .Where(name => foodGroupIdMap.ContainsKey(name))
                .Select(name => foodGroupIdMap[name])
                .ToList();
            await _repository.LinkProductFoodGroupsAsync(code, foodGroupIds);
        }

        var allLabelNames = labelsMap.Values.SelectMany(x => x).Distinct().ToList();
        var labelIdMap = await _repository.GetOrCreateLabelsAsync(allLabelNames);
        foreach (var (code, labelNames) in labelsMap)
        {
            var labelIds = labelNames
                .Where(name => labelIdMap.ContainsKey(name))
                .Select(name => labelIdMap[name])
                .ToList();
            await _repository.LinkProductLabelsAsync(code, labelIds);
        }
    }

    private static Product MapProduct(string[] fields, decimal code)
    {
        return new Product
        {
            Code = code,
            Url = CsvSchema.Get(fields, CsvSchema.Column.Url),
            ProductName = CsvSchema.Get(fields, CsvSchema.Column.ProductName),
            ProductBrand = CsvSchema.Get(fields, CsvSchema.Column.Brands),
            ImageUrl = CsvSchema.Get(fields, CsvSchema.Column.ImageUrl),

            nutriScore = CsvSchema.GetInt(fields, CsvSchema.Column.NutriscoreScore),
            NovaGroup = CsvSchema.GetInt(fields, CsvSchema.Column.NovaGroup),
            EnvironmentalScore = CsvSchema.GetDecimal(fields, CsvSchema.Column.EnvironmentalScoreScore),

            Completeness = CsvSchema.GetDecimal(fields, CsvSchema.Column.Completeness),
            LastImageDatetime = CsvSchema.Get(fields, CsvSchema.Column.LastImageDatetime),
            LastModifiedDatetime = CsvSchema.Get(fields, CsvSchema.Column.LastModifiedDatetime),

            EnergyKcal100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.EnergyKcal100g),
            EnergyFromFat100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.EnergyFromFat100g),
            Fat100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Fat100g),
            SaturatedFat100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.SaturatedFat100g),
            TransFat100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.TransFat100g),
            Cholesterol100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Cholesterol100g),
            Carbohydrates100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Carbohydrates100g),
            Sugars100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Sugars100g),
            AddedSugars100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.AddedSugars100g),
            Fiber100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Fiber100g),
            Proteins100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Proteins100g),
            Salt100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Salt100g),
            Sodium100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Sodium100g),
            Alcohol100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Alcohol100g),
            VitaminA100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.VitaminA100g),
            VitaminC100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.VitaminC100g),
            Calcium100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Calcium100g),
            Iron100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Iron100g),
            Magnesium100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Magnesium100g),
            Zinc100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Zinc100g),
            Potassium100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Potassium100g),

            ButyricAcid100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.ButyricAcid100g),
            CaproicAcid100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.CaproicAcid100g),
            CaprylicAcid100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.CaprylicAcid100g),
            CapricAcid100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.CapricAcid100g),
            LauricAcid100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.LauricAcid100g),
            MyristicAcid100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.MyristicAcid100g),
            PalmiticAcid100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.PalmiticAcid100g),
            StearicAcid100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.StearicAcid100g),
            ArachidicAcid100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.ArachidicAcid100g),
            BehenicAcid100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.BehenicAcid100g),
            LignocericAcid100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.LignocericAcid100g),
            CeroticAcid100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.CeroticAcid100g),
            MontanicAcid100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.MontanicAcid100g),
            MelissicAcid100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.MelissicAcid100g),
            UnsaturatedFat100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.UnsaturatedFat100g),
            MonounsaturatedFat100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.MonounsaturatedFat100g),
            Omega9Fat100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Omega9Fat100g),
            PolyunsaturatedFat100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.PolyunsaturatedFat100g),
            Omega3Fat100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Omega3Fat100g),
            Omega6Fat100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Omega6Fat100g),
            AlphaLinolenicAcid100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.AlphaLinolenicAcid100g),
            EicosapentaenoicAcid100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.EicosapentaenoicAcid100g),
            DocosahexaenoicAcid100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.DocosahexaenoicAcid100g),
            LinoleicAcid100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.LinoleicAcid100g),
            ArachidonicAcid100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.ArachidonicAcid100g),
            GammaLinolenicAcid100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.GammaLinolenicAcid100g),
            DihomoGammaLinolenicAcid100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.DihomoGammaLinolenicAcid100g),
            OleicAcid100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.OleicAcid100g),
            ElaidicAcid100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.ElaidicAcid100g),
            GondoicAcid100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.GondoicAcid100g),
            MeadAcid100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.MeadAcid100g),
            ErucicAcid100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.ErucicAcid100g),
            NervonicAcid100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.NervonicAcid100g),

            Sucrose100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Sucrose100g),
            Glucose100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Glucose100g),
            Fructose100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Fructose100g),
            Galactose100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Galactose100g),
            Lactose100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Lactose100g),
            Maltose100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Maltose100g),
            Maltodextrins100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Maltodextrins100g),
            Psicose100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Psicose100g),
            Starch100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Starch100g),
            Polyols100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Polyols100g),
            Erythritol100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Erythritol100g),
            Isomalt100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Isomalt100g),
            Maltitol100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Maltitol100g),
            Sorbitol100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Sorbitol100g),

            SolubleFiber100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.SolubleFiber100g),
            InsolubleFiber100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.InsolubleFiber100g),
            Casein100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Casein100g),
            SerumProteins100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.SerumProteins100g),
            Nucleotides100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Nucleotides100g),

            BetaCarotene100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.BetaCarotene100g),
            VitaminD100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.VitaminD100g),
            VitaminE100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.VitaminE100g),
            VitaminK100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.VitaminK100g),
            VitaminB1_100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.VitaminB1_100g),
            VitaminB2_100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.VitaminB2_100g),
            VitaminPp100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.VitaminPp100g),
            VitaminB6_100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.VitaminB6_100g),
            VitaminB9_100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.VitaminB9_100g),
            Folates100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Folates100g),
            VitaminB12_100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.VitaminB12_100g),
            Biotin100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Biotin100g),
            PantothenicAcid100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.PantothenicAcid100g),

            Silica100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Silica100g),
            Bicarbonate100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Bicarbonate100g),
            Chloride100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Chloride100g),
            Phosphorus100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Phosphorus100g),
            Copper100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Copper100g),
            Manganese100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Manganese100g),
            Fluoride100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Fluoride100g),
            Selenium100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Selenium100g),
            Chromium100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Chromium100g),
            Molybdenum100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Molybdenum100g),
            Iodine100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Iodine100g),

            Caffeine100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Caffeine100g),
            Taurine100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Taurine100g),
            Methylsulfonylmethane100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Methylsulfonylmethane100g),
            Ph100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.Ph100g),
            
            FruitsVegetablesNutsEstimateFromIngredients100g = CsvSchema.GetDecimal(fields, CsvSchema.Column.FruitsVegetablesNutsEstimateFromIngredients100g)
        };
    }

    private static string[] ParseCsvLine(string line)
    {
        var fields = new List<string>();
        var current = new System.Text.StringBuilder();
        var inQuotes = false;

        for (int i = 0; i < line.Length; i++)
        {
            var c = line[i];

            if (c == '"')
            {
                if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    current.Append('"');
                    i++;
                }
                else
                {
                    inQuotes = !inQuotes;
                }
            }
            else if (c == '\t' && !inQuotes)
            {
                fields.Add(current.ToString());
                current.Clear();
            }
            else
            {
                current.Append(c);
            }
        }

        fields.Add(current.ToString());
        return [.. fields];
    }
}
