using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary.Models
{
    [Keyless]
    public class ProductItemDetails
    {
        public string Name { get; set; }
        public int Material { get; set; }
        [Column(TypeName = "decimal(12,2)")]
        public decimal Weight { get; set; }
    }
}
