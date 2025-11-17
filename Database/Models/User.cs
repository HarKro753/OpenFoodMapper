namespace Backend.Database.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<ProductHistory> ProductHistory { get; set; } = new List<ProductHistory>();
    public ICollection<UserFavoriteProduct> FavoriteProducts { get; set; } = new List<UserFavoriteProduct>();
}
