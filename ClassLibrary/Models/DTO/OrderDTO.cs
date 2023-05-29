
namespace ClassLibrary.Models.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int PaymentId { get; set; }
        public int DiscountCodeId { get; set; }
        public string DeliveryStatus { get; set; } = null!;
        public string DeliveryMethod { get; set; } = null!;
        public string OrderStatus { get; set; } = null!;
        public double TotalPrice { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<int> ProductItemIds { get; set; } = new List<int>();
    }
}
