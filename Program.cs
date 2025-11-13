using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using OpenFood.Database;

namespace OpenFood;

class Program
{
    static async Task Main(string[] args)
    {
        var config = new Config();
        var connectionString = config.GetConnectionString();

        var csvFiles = GetCsvFilePaths();
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

    static List<string> GetCsvFilePaths()
    {
        var files = new List<string>();
        var directory = Directory.GetCurrentDirectory();
        var foodDirectory = Path.Combine(directory, "Food");

        // Generate file names from part_aa to part_bp
        for (char first = 'a'; first <= 'b'; first++)
        {
            var endChar = first == 'a' ? 'z' : 'p';
            for (char second = 'a'; second <= endChar; second++)
            {
                var fileName = $"part_{first}{second}";
                var filePath = Path.Combine(foodDirectory, fileName);

                if (File.Exists(filePath))
                {
                    files.Add(filePath);
                }
                else
                {
                    Console.WriteLine($"Warning: File not found: {filePath}");
                }
            }
        }

        return files;
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
            var updates = new List<(decimal code, string? countries)>();

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
                        if (!string.IsNullOrWhiteSpace(countries))
                        {
                            updates.Add((code, countries));
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
                var batchSize = 1000;
                for (int i = 0; i < updates.Count; i += batchSize)
                {
                    var batch = updates.Skip(i).Take(batchSize).ToList();

                    var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
                    optionsBuilder.UseNpgsql(connectionString);

                    using var context = new DatabaseContext(optionsBuilder.Options);

                    foreach (var (code, countries) in batch)
                    {
                        var product = await context.Products.FindAsync(code);
                        if (product != null)
                        {
                            if (!string.IsNullOrWhiteSpace(countries))
                            {
                                product.Countries = countries;
                            }
                            updated++;
                        }
                        else
                        {
                            skipped++;
                        }
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
