using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Models
{
    public class PaymentForm
    {
        public int Id { get; set; }
        public string DeliveryMethod { get; set; }
        public string PaymentMethod { get; set; }
    }
}
