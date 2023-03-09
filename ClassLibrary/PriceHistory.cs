using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    class PriceHistory
    {
        public int Id { get; set; }

        public int ProductItemId { get; set; }

        public DateTime ChangeDate { get; set; }
        public decimal Price { get; set; }

        public ProductItem ProductItem { get; set; }
    }
}
