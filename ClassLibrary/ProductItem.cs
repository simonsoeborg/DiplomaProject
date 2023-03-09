
namespace ClassLibrary
{
    public class ProductItem
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public string Condition { get; set; } = null!;
        public string Quality { get; set; } = null!;
        public sbyte Sold { get; set; }
        public decimal? Weight { get; set; }

        public decimal PurchasePrice { get; set; }
        public decimal CurrentPrice { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime SoldDate { get; set; }


        public Product Product { get; set; }
        public ICollection<PriceHistory> PriceHistories { get; set; }

    }
}
