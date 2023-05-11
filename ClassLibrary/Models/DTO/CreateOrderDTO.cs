using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Models.DTO
{
    public class CreateOrderDTO
    {
        public int Id { get; set; }
        public Customer Customer { get; set; }
        public PaymentForm PaymentForm { get; set; }
        public List<ProductItemWeb> ProductItemWeb { get; set; }
        [Column(TypeName = "decimal(12,2)")]
        public decimal TotalPrice { get; set; }
    }
}
