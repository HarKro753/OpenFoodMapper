namespace Backend.Database.Models;

public class Additive
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public RiskLevel? Risk { get; set; }
    public int? AdditiveTypeId { get; set; }

    // Navigation properties
    public AdditiveType? AdditiveType { get; set; }
    public ICollection<AdditiveHealthRisk> AdditiveHealthRisks { get; set; } = new List<AdditiveHealthRisk>();
}

public enum RiskLevel
{
    NoRisk = 0,
    LowRisk = 1,
    MediumRisk = 2,
    HighRisk = 3
}