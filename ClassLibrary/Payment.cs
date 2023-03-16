
namespace ClassLibrary
{
    public class Payment
    {
        public int Id { get; set; }
        public DateTime DatePaid { get; set; }
        public double? Amount { get; set; }
        public sbyte? Approved { get; set; }
        public string? Method { get; set; }

    }
}
