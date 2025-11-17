namespace OpenFood.Database.Models;

public class Label
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<ProductLabel> ProductLabels { get; set; } = new List<ProductLabel>();
}
