namespace Backend.Database.Models;

public class ProductIngredient
{
    public decimal ProductCode { get; set; }
    public int IngredientId { get; set; }

    // Navigation properties
    public Product Product { get; set; } = null!;
    public Ingredient Ingredient { get; set; } = null!;
}
