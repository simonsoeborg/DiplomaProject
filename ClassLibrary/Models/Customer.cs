namespace ClassLibrary.Models
{
    public class Customer
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Phone { get; set; }

        public string Email { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }

        public virtual ICollection<Order> Orders { get; } = new List<Order>();
    }
}
