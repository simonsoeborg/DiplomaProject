namespace ClassLibrary.Models
{
    public class DiscountCode
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public int DiscountPercentage { get; set; }
    }
}
