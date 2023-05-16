using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary.Models
{
    public class ProductItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; } = null!;
        public ConditionType Condition { get; set; }
        public QualityType Quality { get; set; }
        public sbyte Sold { get; set; }
        [Column(TypeName = "decimal(12,2)")]
        public decimal? Weight { get; set; }
        [Column(TypeName = "decimal(12,2)")]
        public decimal PurchasePrice { get; set; }
        [Column(TypeName = "decimal(12,2)")]
        public decimal CurrentPrice { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? SoldDate { get; set; }
        public string? CustomText { get; set; }
        public ICollection<PriceHistory>? PriceHistories { get; set; } = new List<PriceHistory>();
        public ICollection<Image> Images { get; set; } = new List<Image>();

    }
}