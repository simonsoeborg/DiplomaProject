using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary.Models.DTO
{
    public class CreateOrderDTO
    {
        public Customer Customer { get; set; } = null!;
        public DiscountCode DiscountCode { get; set; } = null!;
        public Payment Payment { get; set; } = null!;
        [Column(TypeName = "decimal(12,2)")]
        public List<int> ProductItemIds { get; set; } = new List<int>();
    }
}
