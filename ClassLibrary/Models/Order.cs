namespace ClassLibrary.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;
        public int PaymentId { get; set; }
        public Payment? Payment { get; set; }
        public int? DiscountCodeId { get; set; }
        public DiscountCode? DiscountCode { get; set; } = null!;
        public string DeliveryStatus { get; set; } = null!;
        public string OrderStatus { get; set; } = null!;
        public double TotalPrice { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<ProductItem> ProductItems { get; set; } = new List<ProductItem>();
    }
}
