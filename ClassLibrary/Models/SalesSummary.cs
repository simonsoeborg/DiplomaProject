using Microsoft.EntityFrameworkCore;

namespace ClassLibrary.Models
{
    [Keyless]
    public class SalesSummary
    {
        public int TotalSales { get; set; }
        public double TotalAmount { get; set; }
    }
}
