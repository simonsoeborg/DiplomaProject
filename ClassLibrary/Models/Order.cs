﻿namespace ClassLibrary.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ProductItemId { get; set; }
        public string PaymentStatus { get; set; } = null!;
        public string DeliveryStatus { get; set; } = null!;
        public string? DiscountCode { get; set; }
        public bool Active { get; set; }
        public ICollection<OrderElements> OrderElements { get; } = new List<OrderElements>(); // Collection navigation containing dependents
    }
}
