namespace Backend.Database.Models;

public class ProductAdditive
{
    public decimal ProductCode { get; set; }
    public int AdditiveId { get; set; }

    // Navigation properties
    public Product Product { get; set; } = null!;
    public Additive Additive { get; set; } = null!;
}
