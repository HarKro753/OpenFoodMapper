namespace OpenFood.Database.Models;

public class ProductFoodGroup
{
    public decimal ProductCode { get; set; }
    public int FoodGroupId { get; set; }

    public Product Product { get; set; } = null!;
    public FoodGroup FoodGroup { get; set; } = null!;
}
