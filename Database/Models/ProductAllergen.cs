namespace OpenFood.Database.Models;

public class ProductAllergen
{
    public decimal ProductCode { get; set; }
    public int AllergenId { get; set; }

    public Product Product { get; set; } = null!;
    public Allergen Allergen { get; set; } = null!;
}
