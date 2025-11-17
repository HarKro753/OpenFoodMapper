namespace Backend.Database.Models;

public class Allergen
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<ProductAllergen> ProductAllergens { get; set; } = new List<ProductAllergen>();
}
