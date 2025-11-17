namespace Backend.Database.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    // Navigation properties
    public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
}
