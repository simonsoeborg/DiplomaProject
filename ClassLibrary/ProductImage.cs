
namespace ClassLibrary
{
    public class ProductImage
    {
        public int ImageId { get; set; }
        public Image Image { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
