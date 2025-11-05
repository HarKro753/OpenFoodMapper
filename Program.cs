using System.Diagnostics;
using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using Npgsql;

class Program
{
    const string DB_HOST = "192.168.178.186";
    const string DB_PORT = "5432";
    const string DB_NAME = "mydb";
    const string DB_USER = "myuser";
    const string DB_PASSWORD = "1234";
    const int MAX_WORKERS = 16;

    static readonly string[] COLUMN_NAMES = {
        "code", "url", "creator", "created_t", "created_datetime", "last_modified_t", "last_modified_datetime",
        "last_modified_by", "last_updated_t", "last_updated_datetime", "product_name", "abbreviated_product_name",
        "generic_name", "quantity", "packaging", "packaging_tags", "packaging_en", "packaging_text", "brands",
        "brands_tags", "brands_en", "categories", "categories_tags", "categories_en", "origins", "origins_tags",
        "origins_en", "manufacturing_places", "manufacturing_places_tags", "labels", "labels_tags", "labels_en",
        "emb_codes", "emb_codes_tags", "first_packaging_code_geo", "cities", "cities_tags", "purchase_places",
        "stores", "countries", "countries_tags", "countries_en", "ingredients_text", "ingredients_tags",
        "ingredients_analysis_tags", "allergens", "allergens_en", "traces", "traces_tags", "traces_en",
        "serving_size", "serving_quantity", "no_nutrition_data", "additives_n", "additives", "additives_tags",
        "additives_en", "nutriscore_score", "nutriscore_grade", "nova_group", "pnns_groups_1", "pnns_groups_2",
        "food_groups", "food_groups_tags", "food_groups_en", "states", "states_tags", "states_en", "brand_owner",
        "environmental_score_score", "environmental_score_grade", "nutrient_levels_tags", "product_quantity",
        "owner", "data_quality_errors_tags", "unique_scans_n", "popularity_tags", "completeness", "last_image_t",
        "last_image_datetime", "main_category", "main_category_en", "image_url", "image_small_url",
        "image_ingredients_url", "image_ingredients_small_url", "image_nutrition_url", "image_nutrition_small_url",
        "energy-kj_100g", "energy-kcal_100g", "energy_100g", "energy-from-fat_100g", "fat_100g",
        "saturated-fat_100g", "butyric-acid_100g", "caproic-acid_100g", "caprylic-acid_100g", "capric-acid_100g",
        "lauric-acid_100g", "myristic-acid_100g", "palmitic-acid_100g", "stearic-acid_100g", "arachidic-acid_100g",
        "behenic-acid_100g", "lignoceric-acid_100g", "cerotic-acid_100g", "montanic-acid_100g", "melissic-acid_100g",
        "unsaturated-fat_100g", "monounsaturated-fat_100g", "omega-9-fat_100g", "polyunsaturated-fat_100g",
        "omega-3-fat_100g", "omega-6-fat_100g", "alpha-linolenic-acid_100g", "eicosapentaenoic-acid_100g",
        "docosahexaenoic-acid_100g", "linoleic-acid_100g", "arachidonic-acid_100g", "gamma-linolenic-acid_100g",
        "dihomo-gamma-linolenic-acid_100g", "oleic-acid_100g", "elaidic-acid_100g", "gondoic-acid_100g",
        "mead-acid_100g", "erucic-acid_100g", "nervonic-acid_100g", "trans-fat_100g", "cholesterol_100g",
        "carbohydrates_100g", "sugars_100g", "added-sugars_100g", "sucrose_100g", "glucose_100g", "fructose_100g",
        "galactose_100g", "lactose_100g", "maltose_100g", "maltodextrins_100g", "psicose_100g", "starch_100g",
        "polyols_100g", "erythritol_100g", "isomalt_100g", "maltitol_100g", "sorbitol_100g", "fiber_100g",
        "soluble-fiber_100g", "insoluble-fiber_100g", "proteins_100g", "casein_100g", "serum-proteins_100g",
        "nucleotides_100g", "salt_100g", "added-salt_100g", "sodium_100g", "alcohol_100g", "vitamin-a_100g",
        "beta-carotene_100g", "vitamin-d_100g", "vitamin-e_100g", "vitamin-k_100g", "vitamin-c_100g",
        "vitamin-b1_100g", "vitamin-b2_100g", "vitamin-pp_100g", "vitamin-b6_100g", "vitamin-b9_100g",
        "folates_100g", "vitamin-b12_100g", "biotin_100g", "pantothenic-acid_100g", "silica_100g",
        "bicarbonate_100g", "potassium_100g", "chloride_100g", "calcium_100g", "phosphorus_100g", "iron_100g",
        "magnesium_100g", "zinc_100g", "copper_100g", "manganese_100g", "fluoride_100g", "selenium_100g",
        "chromium_100g", "molybdenum_100g", "iodine_100g", "caffeine_100g", "taurine_100g",
        "methylsulfonylmethane_100g", "ph_100g", "fruits-vegetables-nuts_100g", "fruits-vegetables-nuts-dried_100g",
        "fruits-vegetables-nuts-estimate_100g", "fruits-vegetables-nuts-estimate-from-ingredients_100g",
        "collagen-meat-protein-ratio_100g", "cocoa_100g", "chlorophyl_100g", "carbon-footprint_100g",
        "carbon-footprint-from-meat-or-fish_100g", "nutrition-score-fr_100g", "nutrition-score-uk_100g",
        "glycemic-index_100g", "water-hardness_100g", "choline_100g", "phylloquinone_100g", "beta-glucan_100g",
        "inositol_100g", "carnitine_100g", "sulphate_100g", "nitrate_100g", "acidity_100g", "carbohydrates-total_100g"
    };

    static readonly object progressLock = new object();
    static long totalRowsUploaded = 0;
    static int successfulUploads = 0;

    static async Task Main(string[] args)
    {
        var startTime = DateTime.Now;
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Starting parallel upload process...");
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Using {MAX_WORKERS} parallel workers");

        var connectionString = $"Host={DB_HOST};Port={DB_PORT};Database={DB_NAME};Username={DB_USER};Password={DB_PASSWORD}";

        try
        {
            // Find all part_* files
            var csvFiles = Directory.GetFiles(".", "part_*")
                .OrderBy(f => f)
                .ToArray();

            if (csvFiles.Length == 0)
            {
                Console.WriteLine("ERROR: No part_* files found!");
                return;
            }

            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Found {csvFiles.Length} files to upload");
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Files: {Path.GetFileName(csvFiles[0])} to {Path.GetFileName(csvFiles[^1])}");
            Console.WriteLine(new string('=', 80));

            // Create table with first file
            Console.WriteLine($"\n[{DateTime.Now:HH:mm:ss}] Using predefined column names ({COLUMN_NAMES.Length} columns)");
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Processing first file to create table: {Path.GetFileName(csvFiles[0])}");

            var firstFileStart = DateTime.Now;
            await CreateTableAndUploadFirstFile(connectionString, csvFiles[0]);

            var firstFileDuration = (DateTime.Now - firstFileStart).TotalSeconds;
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] ✓ First file uploaded in {firstFileDuration:F1}s");
            Console.WriteLine(new string('=', 80));

            // Process remaining files in parallel
            var remainingFiles = csvFiles.Skip(1).ToArray();
            var failedFiles = new List<string>();

            if (remainingFiles.Length > 0)
            {
                Console.WriteLine($"\n[{DateTime.Now:HH:mm:ss}] Starting parallel upload of {remainingFiles.Length} remaining files...");
                Console.WriteLine(new string('-', 80));

                var semaphore = new SemaphoreSlim(MAX_WORKERS);
                var tasks = remainingFiles.Select(async (file, index) =>
                {
                    await semaphore.WaitAsync();
                    try
                    {
                        return await UploadFile(connectionString, file, index + 2, csvFiles.Length);
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }).ToArray();

                var results = await Task.WhenAll(tasks);
                failedFiles.AddRange(results.Where(r => !r.Success).Select(r => r.FileName));
            }

            // Summary
            Console.WriteLine("\n" + new string('=', 80));
            Console.WriteLine("UPLOAD SUMMARY");
            Console.WriteLine(new string('=', 80));
            var totalDuration = (DateTime.Now - startTime).TotalSeconds;
            Console.WriteLine($"Total time: {totalDuration:F1}s ({totalDuration / 60:F1} minutes)");
            Console.WriteLine($"Successful files: {successfulUploads}/{csvFiles.Length}");
            Console.WriteLine($"Total rows uploaded: {totalRowsUploaded:N0}");

            if (failedFiles.Count > 0)
            {
                Console.WriteLine($"\nFailed files ({failedFiles.Count}):");
                foreach (var file in failedFiles)
                    Console.WriteLine($"  - {file}");
            }
            else
            {
                Console.WriteLine("\n✓ All files uploaded successfully!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nFATAL ERROR: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }

    static async Task CreateTableAndUploadFirstFile(string connectionString, string filePath)
    {
        await using var conn = new NpgsqlConnection(connectionString);
        await conn.OpenAsync();

        // Drop and create table
        var columnDefs = string.Join(", ", COLUMN_NAMES.Select(c => $"\"{c}\" TEXT"));
        var createTableSql = $"DROP TABLE IF EXISTS products; CREATE TABLE products ({columnDefs});";

        await using (var cmd = new NpgsqlCommand(createTableSql, conn))
        {
            await cmd.ExecuteNonQueryAsync();
        }

        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Table created with TEXT columns");

        // Upload first file
        var result = await UploadFileInternal(conn, filePath, 1, 1);

        lock (progressLock)
        {
            totalRowsUploaded = result.RowCount;
            successfulUploads = 1;
        }
    }

    static async Task<(bool Success, string FileName)> UploadFile(string connectionString, string filePath, int fileNum, int totalFiles)
    {
        try
        {
            await using var conn = new NpgsqlConnection(connectionString);
            await conn.OpenAsync();

            await UploadFileInternal(conn, filePath, fileNum, totalFiles);

            return (true, Path.GetFileName(filePath));
        }
        catch (Exception ex)
        {
            lock (progressLock)
            {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] ✗ ERROR uploading {Path.GetFileName(filePath)}: {ex.Message}");
            }
            return (false, Path.GetFileName(filePath));
        }
    }

    static async Task<(long RowCount, double Duration)> UploadFileInternal(NpgsqlConnection conn, string filePath, int fileNum, int totalFiles)
    {
        var fileStart = DateTime.Now;
        var fileInfo = new FileInfo(filePath);
        var fileSizeMb = fileInfo.Length / (1024.0 * 1024.0);

        lock (progressLock)
        {
            Console.WriteLine($"\n[{DateTime.Now:HH:mm:ss}] Processing file {fileNum}/{totalFiles}: {fileInfo.Name}");
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] File size: {fileSizeMb:F2} MB");
        }

        long rowCount = 0;
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = "\t",
            HasHeaderRecord = false,
            BadDataFound = null, // Ignore bad data
            MissingFieldFound = null // Ignore missing fields
        };

        // Use PostgreSQL COPY for bulk insert (much faster!)
        var copyCommand = $"COPY products ({string.Join(", ", COLUMN_NAMES.Select(c => $"\"{c}\""))}) FROM STDIN (FORMAT TEXT, DELIMITER E'\\t', NULL '')";

        await using (var writer = await conn.BeginTextImportAsync(copyCommand))
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, config);

            while (await csv.ReadAsync())
            {
                var values = new List<string>();
                for (int i = 0; i < COLUMN_NAMES.Length; i++)
                {
                    var value = csv.GetField(i);
                    // Escape special characters for PostgreSQL TEXT format
                    if (string.IsNullOrEmpty(value))
                    {
                        values.Add("\\N"); // NULL in PostgreSQL TEXT format
                    }
                    else
                    {
                        // Escape backslashes, tabs, newlines
                        value = value.Replace("\\", "\\\\")
                                    .Replace("\t", "\\t")
                                    .Replace("\n", "\\n")
                                    .Replace("\r", "\\r");
                        values.Add(value);
                    }
                }

                await writer.WriteLineAsync(string.Join("\t", values));
                rowCount++;
            }
        }

        var duration = (DateTime.Now - fileStart).TotalSeconds;

        lock (progressLock)
        {
            totalRowsUploaded += rowCount;
            successfulUploads++;
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [{fileInfo.Name}] Loaded {rowCount:N0} rows");
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] ✓ [{fileInfo.Name}] Uploaded successfully in {duration:F1}s");
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Progress: {successfulUploads}/{totalFiles} files | {totalRowsUploaded:N0} total rows");
        }

        return (rowCount, duration);
    }
}
