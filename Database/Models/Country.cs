namespace OpenFood.Database.Models;

public class Country
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<ProductCountry> ProductCountries { get; set; } = new List<ProductCountry>();
}
