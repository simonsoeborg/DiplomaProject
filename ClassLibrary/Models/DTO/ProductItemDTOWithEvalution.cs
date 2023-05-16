using ClassLibrary.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Models.DTO
{
    public class ProductItemWithEvalution
    {    
        public ProductItem Productitem { get; set; }
        public Product Product { get; set; }
        public decimal SalesValue { get; set; }
    }
}
