
namespace ClassLibrary.Models.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int PaymentId { get; set; }
        public string DeliveryStatus { get; set; } = null!;
        public string? DiscountCode { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<int> OrderElementIDs { get; set; } = new List<int>();
    }
}
