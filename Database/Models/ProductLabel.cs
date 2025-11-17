namespace OpenFood.Database.Models;

public class ProductLabel
{
    public decimal ProductCode { get; set; }
    public int LabelId { get; set; }

    public Product Product { get; set; } = null!;
    public Label Label { get; set; } = null!;
}
