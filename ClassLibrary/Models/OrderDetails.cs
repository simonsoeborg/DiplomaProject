using Microsoft.EntityFrameworkCore;

namespace ClassLibrary.Models
{
    [Keyless]
    public class OrderDetails
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int PaymentId { get; set; }
        public int ProductId { get; set; }
        public int ProductItemId { get; set; }
        public string PaymentStatus { get; set; }
        public string DeliveryStatus { get; set; }
        public string? DiscountCode { get; set; }
        public bool Active { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public ICollection<OrderElements> OrderElements { get; set; } = new List<OrderElements>(); // Collection navigation containing dependents

    }
}
