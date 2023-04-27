using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Models
{
    public class OrderElementsDTO
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductItemId { get; set; }
    }
}
