using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.PaymentModels
{
    public class StripePaymentModel
    {
        public string Currency { get; set; }
        public int Amount { get; set; }
        public string PaymentMethodId { get; set; }
        public int CustomerId { get; set; }
    }
}
