using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using OpenFood.Database.Models.Backend.Database;

namespace OpenFood;

class Program
{
    static async Task Main(string[] args)
    {
        var config = new Config();
        var connectionString = config.GetConnectionString();

        var csvFiles = Tools.GetCsvFilePaths();
        Console.WriteLine($"Found {csvFiles.Count} CSV files to process");

        // Process files concurrently
        var batchSize = config.MaxWorkers;
        var totalUpdated = 0;
        var totalSkipped = 0;
        var totalErrors = 0;

        var semaphore = new SemaphoreSlim(batchSize);
        var tasks = new List<Task<(int updated, int skipped, int errors)>>();

        foreach (var csvFile in csvFiles)
        {
            await semaphore.WaitAsync();

            var task = Task.Run(async () =>
            {
                try
                {
                    return await ProcessCsvFile(csvFile, connectionString);
                }
                finally
                {
                    semaphore.Release();
                }
            });

            tasks.Add(task);
        }

        var results = await Task.WhenAll(tasks);

        foreach (var result in results)
        {
            totalUpdated += result.updated;
            totalSkipped += result.skipped;
            totalErrors += result.errors;
        }

        Console.WriteLine("\n=== Import Complete ===");
        Console.WriteLine($"Total updated: {totalUpdated}");
        Console.WriteLine($"Total skipped: {totalSkipped}");
        Console.WriteLine($"Total errors: {totalErrors}");
    }

    

    static async Task<(int updated, int skipped, int errors)> ProcessCsvFile(string csvFile, string connectionString)
    {
        var fileName = Path.GetFileName(csvFile);
        Console.WriteLine($"[{fileName}] Starting processing...");

        var updated = 0;
        var skipped = 0;
        var errors = 0;

        try
        {
            var updates = new List<(decimal code, string? categoriesEn, string? countries)>();

            // Read the CSV file
            using (var reader = new StreamReader(csvFile))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
                Delimiter = "\t",
                BadDataFound = null,
                MissingFieldFound = null
            }))
            {
                await csv.ReadAsync();

                while (await csv.ReadAsync())
                {
                    try
                    {
                        // Index 0 is code, index 23 is categories_en, index 39 is countries according to CsvSchema.cs
                        var codeStr = csv.GetField(0);
                        var categoriesEn = csv.GetField(23);
                        var countries = csv.GetField(39);

                        if (string.IsNullOrWhiteSpace(codeStr))
                        {
                            skipped++;
                            continue;
                        }

                        if (!decimal.TryParse(codeStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var code))
                        {
                            skipped++;
                            continue;
                        }

                        // Only add if at least one field has a value
                        if (!string.IsNullOrWhiteSpace(categoriesEn) || !string.IsNullOrWhiteSpace(countries))
                        {
                            updates.Add((code, categoriesEn, countries));
                        }
                        else
                        {
                            skipped++;
                        }
                    }
                    catch (Exception ex)
                    {
                        errors++;
                        Console.WriteLine($"[{fileName}] Error reading row: {ex.Message}");
                    }
                }
            }

            // Batch update the database
            if (updates.Count > 0)
            {
                var batchSize = 500;
                for (int i = 0; i < updates.Count; i += batchSize)
                {
                    var batch = updates.Skip(i).Take(batchSize).ToList();

                    var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
                    optionsBuilder.UseNpgsql(connectionString);

                    using var context = new DatabaseContext(optionsBuilder.Options);

                    // Track relationships we're adding in this batch to avoid duplicates
                    var addedProductCategories = new HashSet<(decimal, int)>();
                    var addedProductCountries = new HashSet<(decimal, int)>();

                    foreach (var (code, categoriesEn, countries) in batch)
                    {
                        var product = await context.Products.FindAsync(code);
                        if (product == null)
                        {
                            skipped++;
                            continue;
                        }

                        // Update the raw columns
                        if (!string.IsNullOrWhiteSpace(categoriesEn))
                        {
                            product.CategoriesEn = categoriesEn;

                            // Parse and insert categories (deduplicate within the product)
                            var categoryNames = Tools.ParseCommaSeparatedValues(categoriesEn).Distinct().ToList();
                            foreach (var categoryName in categoryNames)
                            {
                                var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Name == categoryName);
                                if (category == null)
                                {
                                    category = new Database.Models.Category { Name = categoryName };
                                    context.Categories.Add(category);
                                    await context.SaveChangesAsync(); // Save to get the ID
                                    context.Entry(category).State = EntityState.Detached; // Detach to avoid tracking issues
                                }

                                // Check if relationship already exists in DB or in current batch
                                var relationKey = (code, category.Id);
                                if (!addedProductCategories.Contains(relationKey))
                                {
                                    var existingRelation = await context.ProductCategories.AsNoTracking()
                                        .AnyAsync(pc => pc.ProductCode == code && pc.CategoryId == category.Id);

                                    if (!existingRelation)
                                    {
                                        context.ProductCategories.Add(new Database.Models.ProductCategory
                                        {
                                            ProductCode = code,
                                            CategoryId = category.Id
                                        });
                                        addedProductCategories.Add(relationKey);
                                    }
                                }
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(countries))
                        {
                            product.Countries = countries;

                            // Parse and insert countries (deduplicate within the product)
                            var countryNames = Tools.ParseCommaSeparatedValues(countries).Distinct().ToList();
                            foreach (var countryName in countryNames)
                            {
                                var country = await context.Countries.AsNoTracking().FirstOrDefaultAsync(c => c.Name == countryName);
                                if (country == null)
                                {
                                    country = new Database.Models.Country { Name = countryName };
                                    context.Countries.Add(country);
                                    await context.SaveChangesAsync(); // Save to get the ID
                                    context.Entry(country).State = EntityState.Detached; // Detach to avoid tracking issues
                                }

                                // Check if relationship already exists in DB or in current batch
                                var relationKey = (code, country.Id);
                                if (!addedProductCountries.Contains(relationKey))
                                {
                                    var existingRelation = await context.ProductCountries.AsNoTracking()
                                        .AnyAsync(pc => pc.ProductCode == code && pc.CountryId == country.Id);

                                    if (!existingRelation)
                                    {
                                        context.ProductCountries.Add(new Database.Models.ProductCountry
                                        {
                                            ProductCode = code,
                                            CountryId = country.Id
                                        });
                                        addedProductCountries.Add(relationKey);
                                    }
                                }
                            }
                        }

                        updated++;
                    }

                    await context.SaveChangesAsync();
                }
            }

            Console.WriteLine($"[{fileName}] Completed - Updated: {updated}, Skipped: {skipped}, Errors: {errors}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[{fileName}] Fatal error: {ex.Message}");
            errors++;
        }

        return (updated, skipped, errors);
    }
}
