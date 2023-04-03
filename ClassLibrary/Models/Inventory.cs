using Microsoft.EntityFrameworkCore;

namespace ClassLibrary.Models
{
    [Keyless]
    public class Inventory
    {
        public string Name { get; set; }
        public int TotalProducts { get; set; }
    }
}
