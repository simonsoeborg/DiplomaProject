using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary.Models.DTO
{
    public class CreateOrderDTO
    {
        public Customer Customer { get; set; }
        public PaymentForm PaymentForm { get; set; }
        [Column(TypeName = "decimal(12,2)")]
        public List<int> ProductItemIds { get; set; }
    }
}
