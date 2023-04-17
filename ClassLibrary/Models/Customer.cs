namespace ClassLibrary.Models
{
    public class Customer
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public int? Phone { get; set; }

        public string? Email { get; set; }

        public virtual ICollection<Order> Orders { get; } = new List<Order>();
    }
}
