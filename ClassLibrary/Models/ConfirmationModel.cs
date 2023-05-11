using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Models
{
    public class ConfirmationModel
    {
        public int Id { get; set; }
        public Order Order { get; set; }
        public Customer Customer { get; set; }
        public List<ProductItem> ProductItems { get; set; }
        public Payment Payment { get; set; }
    }
}
