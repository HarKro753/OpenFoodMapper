namespace Backend.Database.Models;

public class AdditiveHealthRisk
{
    public int AdditiveId { get; set; }
    public int HealthRiskId { get; set; }

    // Navigation properties
    public Additive Additive { get; set; } = null!;
    public HealthRisk HealthRisk { get; set; } = null!;
}
