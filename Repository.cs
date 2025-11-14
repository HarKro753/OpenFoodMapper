using OpenFood.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace OpenFood;

public class Repository
{
    private readonly DatabaseContext _db;

    public Repository(DatabaseContext db)
    {
        _db = db;
    }

    public async Task<Product?> GetProductByCodeAsync(decimal code)
    {
        return await _db.Products.FindAsync(code);
    }

    public async Task UpsertProductAsync(Product product)
    {
        var existingProduct = await _db.Products.FindAsync(product.Code);
        if (existingProduct == null)
        {
            await _db.Products.AddAsync(product);
        }
        else
        {
            _db.Entry(existingProduct).CurrentValues.SetValues(product);
        }
    }

    public async Task UpsertProductsAsync(List<Product> products)
    {
        foreach (var product in products)
        {
            await UpsertProductAsync(product);
        }
        await _db.SaveChangesAsync();
    }

    public async Task<Dictionary<string, int>> GetOrCreateCategoriesAsync(List<string> categoryNames)
    {
        foreach (var name in categoryNames)
        {
            await _db.Database.ExecuteSqlRawAsync(
                "INSERT INTO categories (name) VALUES ({0}) ON CONFLICT (name) DO NOTHING", name);
        }

        return await _db.Categories
            .Where(c => categoryNames.Contains(c.Name))
            .ToDictionaryAsync(c => c.Name, c => c.Id);
    }

    public async Task<Dictionary<string, int>> GetOrCreateAdditivesAsync(List<string> additiveNames)
    {
        foreach (var name in additiveNames)
        {
            await _db.Database.ExecuteSqlRawAsync(
                "INSERT INTO additives (name) VALUES ({0}) ON CONFLICT (name) DO NOTHING", name);
        }

        return await _db.Additives
            .Where(a => additiveNames.Contains(a.Name))
            .ToDictionaryAsync(a => a.Name, a => a.Id);
    }

    public async Task<Dictionary<string, int>> GetOrCreateIngredientsAsync(List<string> ingredientNames)
    {
        foreach (var name in ingredientNames)
        {
            await _db.Database.ExecuteSqlRawAsync(
                "INSERT INTO ingredients (name, is_vegetable) VALUES ({0}, {1}) ON CONFLICT (name) DO NOTHING",
                name, false);
        }

        return await _db.Ingredients
            .Where(i => ingredientNames.Contains(i.Name))
            .ToDictionaryAsync(i => i.Name, i => i.Id);
    }

    public async Task<Dictionary<string, int>> GetOrCreateCountriesAsync(List<string> countryNames)
    {
        foreach (var name in countryNames)
        {
            await _db.Database.ExecuteSqlRawAsync(
                "INSERT INTO countries (name) VALUES ({0}) ON CONFLICT (name) DO NOTHING", name);
        }

        return await _db.Countries
            .Where(c => countryNames.Contains(c.Name))
            .ToDictionaryAsync(c => c.Name, c => c.Id);
    }

    public async Task LinkProductCategoriesAsync(decimal productCode, List<int> categoryIds)
    {
        foreach (var categoryId in categoryIds)
        {
            await _db.Database.ExecuteSqlRawAsync(
                "INSERT INTO product_categories (product_code, category_id) VALUES ({0}, {1}) ON CONFLICT DO NOTHING",
                productCode, categoryId);
        }
    }

    public async Task LinkProductAdditivesAsync(decimal productCode, List<int> additiveIds)
    {
        foreach (var additiveId in additiveIds)
        {
            await _db.Database.ExecuteSqlRawAsync(
                "INSERT INTO product_additives (product_code, additive_id) VALUES ({0}, {1}) ON CONFLICT DO NOTHING",
                productCode, additiveId);
        }
    }

    public async Task LinkProductIngredientsAsync(decimal productCode, List<int> ingredientIds)
    {
        foreach (var ingredientId in ingredientIds)
        {
            await _db.Database.ExecuteSqlRawAsync(
                "INSERT INTO product_ingredients (product_code, ingredient_id) VALUES ({0}, {1}) ON CONFLICT DO NOTHING",
                productCode, ingredientId);
        }
    }

    public async Task LinkProductCountriesAsync(decimal productCode, List<int> countryIds)
    {
        foreach (var countryId in countryIds)
        {
            await _db.Database.ExecuteSqlRawAsync(
                "INSERT INTO product_countries (product_code, country_id) VALUES ({0}, {1}) ON CONFLICT DO NOTHING",
                productCode, countryId);
        }
    }
}
