using System.ComponentModel.DataAnnotations;

namespace GroenlundDBContext.Models
{
    public class Product
    {
        [Key]
        public int HandleId { get; set; }
        public string FieldType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProductImageUrl { get; set; }
        public string Collection { get; set; }
        public string Sku { get; set; }
        public string Ribbon { get; set; }
        public decimal Price { get; set; }
        public decimal Surcharge { get; set; }
        public bool Visible { get; set; }
        public string DiscountMode { get; set; }
        public decimal DiscountValue { get; set; }
        public int Inventory { get; set; }
        public double Weight { get; set; }
        public decimal Cost { get; set; }
    }
}
