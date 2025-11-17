namespace Backend.Database.Models;

public class UserFavoriteProduct
{
    public int UserId { get; set; }
    public decimal ProductCode { get; set; }
    public DateTime FavoritedAt { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
    public Product Product { get; set; } = null!;
}
