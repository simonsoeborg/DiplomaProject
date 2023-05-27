namespace ClassLibrary.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public double Amount { get; set; }
        public string TransactionId { get; set; } = string.Empty;
        public DateTime DatePaid { get; set; }

    }
}
