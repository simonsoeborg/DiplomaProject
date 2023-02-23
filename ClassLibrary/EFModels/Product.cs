namespace ClassLibrary.EFModels;
public partial class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Category { get; set; }
    public int ModelNumber { get; set; }
    public string Material { get; set; } = null!;
    public string? Design { get; set; }
    public string? Dimension { get; set; }
    public virtual Category CategoryNavigation { get; set; } = null!;
    public virtual ICollection<ProductItem> ProductItems { get; } = new List<ProductItem>();
}
