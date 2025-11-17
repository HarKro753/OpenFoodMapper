namespace Backend.Database.Models;

public class ProductHistory
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public decimal ProductCode { get; set; }
    public DateTime ScannedAt { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
    public Product Product { get; set; } = null!;
}
