namespace ClassLibrary.EFModels;

public partial class ProductItem
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int Sku { get; set; }

    public string Condition { get; set; } = null!;

    public string Quality { get; set; } = null!;

    public sbyte Sold { get; set; }

    public decimal? Weight { get; set; }

    public int Quantity { get; set; }

    public virtual Product Product { get; set; } = null!;
}
