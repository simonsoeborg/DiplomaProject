
namespace ClassLibrary.Models
{
    public class OrderElements
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public int ProductItemId { get; set; }
        public ProductItem? ProductItem { get; set; } = null!;
    }
}
