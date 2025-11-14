namespace OpenFood.Database.Models;

public class Ingredient
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public bool IsVegetable { get; set; }

    // Navigation properties
    public ICollection<ProductIngredient> ProductIngredients { get; set; } = new List<ProductIngredient>();
}