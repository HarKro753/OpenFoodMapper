namespace OpenFood;

public class Product
{
    public static readonly (string Name, string Type)[] Columns = {
        // Basic info
        ("code", "BIGINT"), ("url", "TEXT"), ("creator", "TEXT"),
        ("created_t", "BIGINT"), ("created_datetime", "TEXT"),
        ("last_modified_t", "BIGINT"), ("last_modified_datetime", "TEXT"),
        ("last_modified_by", "TEXT"),
        ("last_updated_t", "BIGINT"), ("last_updated_datetime", "TEXT"),

        // Product details
        ("product_name", "TEXT"), ("abbreviated_product_name", "TEXT"),
        ("generic_name", "TEXT"), ("quantity", "TEXT"),

        // Packaging
        ("packaging", "TEXT"), ("packaging_tags", "TEXT"), ("packaging_en", "TEXT"), ("packaging_text", "TEXT"),

        // Brands
        ("brands", "TEXT"), ("brands_tags", "TEXT"), ("brands_en", "TEXT"),

        // Categories
        ("categories", "TEXT"), ("categories_tags", "TEXT"), ("categories_en", "TEXT"),

        // Origins
        ("origins", "TEXT"), ("origins_tags", "TEXT"), ("origins_en", "TEXT"),

        // Manufacturing
        ("manufacturing_places", "TEXT"), ("manufacturing_places_tags", "TEXT"),

        // Labels
        ("labels", "TEXT"), ("labels_tags", "TEXT"), ("labels_en", "TEXT"),

        // Codes
        ("emb_codes", "TEXT"), ("emb_codes_tags", "TEXT"), ("first_packaging_code_geo", "TEXT"),

        // Places
        ("cities", "TEXT"), ("cities_tags", "TEXT"), ("purchase_places", "TEXT"),
        ("stores", "TEXT"), ("countries", "TEXT"), ("countries_tags", "TEXT"), ("countries_en", "TEXT"),

        // Ingredients
        ("ingredients_text", "TEXT"), ("ingredients_tags", "TEXT"),
        ("ingredients_analysis_tags", "TEXT"),

        // Allergens
        ("allergens", "TEXT"), ("allergens_en", "TEXT"),
        ("traces", "TEXT"), ("traces_tags", "TEXT"), ("traces_en", "TEXT"),

        // Serving
        ("serving_size", "TEXT"), ("serving_quantity", "NUMERIC"), ("no_nutrition_data", "TEXT"),

        // Additives
        ("additives_n", "INTEGER"), ("additives", "TEXT"), ("additives_tags", "TEXT"), ("additives_en", "TEXT"),

        // Scores
        ("nutriscore_score", "INTEGER"), ("nutriscore_grade", "TEXT"), ("nova_group", "INTEGER"),
        ("pnns_groups_1", "TEXT"), ("pnns_groups_2", "TEXT"),

        // Food groups
        ("food_groups", "TEXT"), ("food_groups_tags", "TEXT"), ("food_groups_en", "TEXT"),

        // States
        ("states", "TEXT"), ("states_tags", "TEXT"), ("states_en", "TEXT"),

        // Owner
        ("brand_owner", "TEXT"),

        // Environmental
        ("environmental_score_score", "NUMERIC"), ("environmental_score_grade", "TEXT"),

        // Quality
        ("nutrient_levels_tags", "TEXT"), ("product_quantity", "NUMERIC"),
        ("owner", "TEXT"), ("data_quality_errors_tags", "TEXT"),

        // Popularity
        ("unique_scans_n", "INTEGER"), ("popularity_tags", "TEXT"),
        ("completeness", "NUMERIC"),

        // Images
        ("last_image_t", "BIGINT"), ("last_image_datetime", "TEXT"),
        ("main_category", "TEXT"), ("main_category_en", "TEXT"),
        ("image_url", "TEXT"), ("image_small_url", "TEXT"),
        ("image_ingredients_url", "TEXT"), ("image_ingredients_small_url", "TEXT"),
        ("image_nutrition_url", "TEXT"), ("image_nutrition_small_url", "TEXT"),

        // Nutrition - Energy
        ("energy-kj_100g", "NUMERIC"), ("energy-kcal_100g", "NUMERIC"), ("energy_100g", "NUMERIC"),
        ("energy-from-fat_100g", "NUMERIC"),

        // Nutrition - Fats
        ("fat_100g", "NUMERIC"), ("saturated-fat_100g", "NUMERIC"),
        ("butyric-acid_100g", "NUMERIC"), ("caproic-acid_100g", "NUMERIC"), ("caprylic-acid_100g", "NUMERIC"),
        ("capric-acid_100g", "NUMERIC"), ("lauric-acid_100g", "NUMERIC"), ("myristic-acid_100g", "NUMERIC"),
        ("palmitic-acid_100g", "NUMERIC"), ("stearic-acid_100g", "NUMERIC"), ("arachidic-acid_100g", "NUMERIC"),
        ("behenic-acid_100g", "NUMERIC"), ("lignoceric-acid_100g", "NUMERIC"), ("cerotic-acid_100g", "NUMERIC"),
        ("montanic-acid_100g", "NUMERIC"), ("melissic-acid_100g", "NUMERIC"),
        ("unsaturated-fat_100g", "NUMERIC"), ("monounsaturated-fat_100g", "NUMERIC"), ("omega-9-fat_100g", "NUMERIC"),
        ("polyunsaturated-fat_100g", "NUMERIC"), ("omega-3-fat_100g", "NUMERIC"), ("omega-6-fat_100g", "NUMERIC"),
        ("alpha-linolenic-acid_100g", "NUMERIC"), ("eicosapentaenoic-acid_100g", "NUMERIC"),
        ("docosahexaenoic-acid_100g", "NUMERIC"), ("linoleic-acid_100g", "NUMERIC"), ("arachidonic-acid_100g", "NUMERIC"),
        ("gamma-linolenic-acid_100g", "NUMERIC"), ("dihomo-gamma-linolenic-acid_100g", "NUMERIC"),
        ("oleic-acid_100g", "NUMERIC"), ("elaidic-acid_100g", "NUMERIC"), ("gondoic-acid_100g", "NUMERIC"),
        ("mead-acid_100g", "NUMERIC"), ("erucic-acid_100g", "NUMERIC"), ("nervonic-acid_100g", "NUMERIC"),
        ("trans-fat_100g", "NUMERIC"), ("cholesterol_100g", "NUMERIC"),

        // Nutrition - Carbohydrates
        ("carbohydrates_100g", "NUMERIC"), ("sugars_100g", "NUMERIC"), ("added-sugars_100g", "NUMERIC"),
        ("sucrose_100g", "NUMERIC"), ("glucose_100g", "NUMERIC"), ("fructose_100g", "NUMERIC"),
        ("galactose_100g", "NUMERIC"), ("lactose_100g", "NUMERIC"), ("maltose_100g", "NUMERIC"),
        ("maltodextrins_100g", "NUMERIC"), ("psicose_100g", "NUMERIC"), ("starch_100g", "NUMERIC"),
        ("polyols_100g", "NUMERIC"), ("erythritol_100g", "NUMERIC"), ("isomalt_100g", "NUMERIC"),
        ("maltitol_100g", "NUMERIC"), ("sorbitol_100g", "NUMERIC"),

        // Nutrition - Fiber & Proteins
        ("fiber_100g", "NUMERIC"), ("soluble-fiber_100g", "NUMERIC"), ("insoluble-fiber_100g", "NUMERIC"),
        ("proteins_100g", "NUMERIC"), ("casein_100g", "NUMERIC"), ("serum-proteins_100g", "NUMERIC"),
        ("nucleotides_100g", "NUMERIC"),

        // Nutrition - Salt & Sodium
        ("salt_100g", "NUMERIC"), ("added-salt_100g", "NUMERIC"), ("sodium_100g", "NUMERIC"),
        ("alcohol_100g", "NUMERIC"),

        // Nutrition - Vitamins
        ("vitamin-a_100g", "NUMERIC"), ("beta-carotene_100g", "NUMERIC"), ("vitamin-d_100g", "NUMERIC"),
        ("vitamin-e_100g", "NUMERIC"), ("vitamin-k_100g", "NUMERIC"), ("vitamin-c_100g", "NUMERIC"),
        ("vitamin-b1_100g", "NUMERIC"), ("vitamin-b2_100g", "NUMERIC"), ("vitamin-pp_100g", "NUMERIC"),
        ("vitamin-b6_100g", "NUMERIC"), ("vitamin-b9_100g", "NUMERIC"), ("folates_100g", "NUMERIC"),
        ("vitamin-b12_100g", "NUMERIC"), ("biotin_100g", "NUMERIC"), ("pantothenic-acid_100g", "NUMERIC"),

        // Nutrition - Minerals
        ("silica_100g", "NUMERIC"), ("bicarbonate_100g", "NUMERIC"), ("potassium_100g", "NUMERIC"),
        ("chloride_100g", "NUMERIC"), ("calcium_100g", "NUMERIC"), ("phosphorus_100g", "NUMERIC"),
        ("iron_100g", "NUMERIC"), ("magnesium_100g", "NUMERIC"), ("zinc_100g", "NUMERIC"),
        ("copper_100g", "NUMERIC"), ("manganese_100g", "NUMERIC"), ("fluoride_100g", "NUMERIC"),
        ("selenium_100g", "NUMERIC"), ("chromium_100g", "NUMERIC"), ("molybdenum_100g", "NUMERIC"),
        ("iodine_100g", "NUMERIC"),

        // Nutrition - Other
        ("caffeine_100g", "NUMERIC"), ("taurine_100g", "NUMERIC"),
        ("methylsulfonylmethane_100g", "NUMERIC"), ("ph_100g", "NUMERIC"),

        // Nutrition - Fruits/Vegetables
        ("fruits-vegetables-nuts_100g", "NUMERIC"), ("fruits-vegetables-nuts-dried_100g", "NUMERIC"),
        ("fruits-vegetables-nuts-estimate_100g", "NUMERIC"),
        ("fruits-vegetables-nuts-estimate-from-ingredients_100g", "NUMERIC"),

        // Nutrition - Misc
        ("collagen-meat-protein-ratio_100g", "NUMERIC"), ("cocoa_100g", "NUMERIC"), ("chlorophyl_100g", "NUMERIC"),
        ("carbon-footprint_100g", "NUMERIC"), ("carbon-footprint-from-meat-or-fish_100g", "NUMERIC"),
        ("nutrition-score-fr_100g", "INTEGER"), ("nutrition-score-uk_100g", "INTEGER"),
        ("glycemic-index_100g", "NUMERIC"), ("water-hardness_100g", "NUMERIC"), ("choline_100g", "NUMERIC"),
        ("phylloquinone_100g", "NUMERIC"), ("beta-glucan_100g", "NUMERIC"),
        ("inositol_100g", "NUMERIC"), ("carnitine_100g", "NUMERIC"), ("sulphate_100g", "NUMERIC"),
        ("nitrate_100g", "NUMERIC"), ("acidity_100g", "NUMERIC"), ("carbohydrates-total_100g", "NUMERIC")
    };

    public static string[] ColumnNames => Columns.Select(c => c.Name).ToArray();
}
