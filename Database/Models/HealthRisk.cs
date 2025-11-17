namespace Backend.Database.Models;

public class HealthRisk
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    // Navigation properties
    public ICollection<AdditiveHealthRisk> AdditiveHealthRisks { get; set; } = new List<AdditiveHealthRisk>();
}
