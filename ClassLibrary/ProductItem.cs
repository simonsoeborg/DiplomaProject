
namespace ClassLibrary
{
    public class ProductItem
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public ConditionType Condition { get; set; }
        public QualityType Quality { get; set; }
        public sbyte Sold { get; set; }
        public decimal? Weight { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime SoldDate { get; set; }
        public string CustomText { get; set; }
        public ICollection<PriceHistory> PriceHistories { get; set; }
        public ICollection<Image> Images { get; set; }

    }

    public enum QualityType
    {
        FirstQuality = 1,
        SecondQuality = 2,
        ThirdQuality = 3,
    }

    public enum ConditionType
    {
        NoShards = 1,
        FewShards = 2,
        ManyShards = 3,
    }
}
