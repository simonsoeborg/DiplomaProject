
namespace ClassLibrary
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int PaymentId { get; set; }
        public string PaymentStatus { get; set; } = null!;
        public string DeliveryStatus { get; set; } = null!;
        public string? DiscountCode { get; set; }
        public sbyte Active { get; set; }

    }
}
