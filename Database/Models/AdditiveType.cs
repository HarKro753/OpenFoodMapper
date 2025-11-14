namespace OpenFood.Database.Models;

public class AdditiveType
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    // Navigation properties
    public ICollection<Additive> Additives { get; set; } = new List<Additive>();
}
