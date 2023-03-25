namespace ClassLibrary.Models
{
    public class PriceHistory
    {
        public int Id { get; set; }
        public DateTime ChangeDate { get; set; }
        public decimal Price { get; set; }
        public int ProductItemId { get; set; }
        public ProductItem ProductItem { get; set; } = null!;
    }
}
