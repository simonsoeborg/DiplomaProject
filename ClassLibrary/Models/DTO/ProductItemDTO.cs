
using ClassLibrary.Models;

namespace ClassLibrary.DTOModels
{
    public class ProductItemDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public ConditionType Condition { get; set; }
        public QualityType Quality { get; set; }
        public bool Sold { get; set; }
        public decimal? Weight { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? SoldDate { get; set; }
        public string? CustomText { get; set; } = string.Empty;
        public string[] ImageUrls { get; set; } = null!;
        public int[] ImageIds { get; set; } = null!;
        public int[] PriceHistoryIds { get; set; } = null!;
    }
}
