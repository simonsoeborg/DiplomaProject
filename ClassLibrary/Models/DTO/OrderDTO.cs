using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Models.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int PaymentId { get; set; }
        public string PaymentStatus { get; set; } = null!;
        public string DeliveryStatus { get; set; } = null!;
        public string? DiscountCode { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<int> OrderElementIDs { get; set; }
    }
}
