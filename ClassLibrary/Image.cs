
namespace ClassLibrary
{
    public class Image
    {
        public int Id { get; set; }
        public int ProductItemId { get; set; }

        public string Url { get; set; } = null!;

        public ProductItem ProductItem { get; set; }
    }
}
