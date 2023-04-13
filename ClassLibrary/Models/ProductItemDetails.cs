using Microsoft.EntityFrameworkCore;

namespace ClassLibrary.Models
{
    [Keyless]
    public class ProductItemDetails
    {
        public string Name { get; set; }
        public int Material { get; set; }
        public decimal Weight { get; set; }
    }
}
