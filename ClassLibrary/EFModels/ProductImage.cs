namespace ClassLibrary.EFModels;

public partial class ProductImage
{
    public int ImageId { get; set; }

    public int ProductId { get; set; }

    public virtual Image Image { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
