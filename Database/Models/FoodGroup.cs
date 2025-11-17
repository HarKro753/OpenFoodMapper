namespace Backend.Database.Models;

public class FoodGroup
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<ProductFoodGroup> ProductFoodGroups { get; set; } = new List<ProductFoodGroup>();
}
