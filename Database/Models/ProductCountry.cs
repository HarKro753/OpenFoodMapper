namespace OpenFood.Database.Models;

public class ProductCountry
{
    public decimal ProductCode { get; set; }
    public int CountryId { get; set; }

    public Product Product { get; set; } = null!;
    public Country Country { get; set; } = null!;
}
