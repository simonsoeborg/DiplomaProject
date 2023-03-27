
using ClassLibrary.Models;

namespace ClassLibrary.DTOModels
{
    public class ProductItemDTO
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public ConditionType Condition { get; set; }
        public QualityType Quality { get; set; }
        public decimal? Weight { get; set; }
        public string CustomText { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public string[] Images { get; set; } = null!;

    }
}
