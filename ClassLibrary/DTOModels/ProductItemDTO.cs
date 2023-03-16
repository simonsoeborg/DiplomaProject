
namespace ClassLibrary.DTOModels
{
    public class ProductItemDTO
    {
        public int Id { get; set; }
        public DateTime SoldDate { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public ConditionType Condition { get; set; }
        public QualityType Quality { get; set; }
        public sbyte Sold { get; set; }
        public decimal? Weight { get; set; }
        public string CustomText { get; set; }
        public Product Product { get; set; }
        public string[] Images { get; set; }

    }
}
