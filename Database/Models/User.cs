namespace Backend.Database.Models;

public class User
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? AppleId { get; set; }
    
    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }

    // RevenueCat / Premium
    public string? RevenuecatId { get; set; }
    public bool IsPremium { get; set; } = false;
    public DateTime? PremiumExpiration { get; set; }
    public string? LastRevenuecatEvent { get; set; }

    // Navigation properties
    public ICollection<ProductHistory> ProductHistory { get; set; } = new List<ProductHistory>();
    public ICollection<UserFavoriteProduct> FavoriteProducts { get; set; } = new List<UserFavoriteProduct>();
}
