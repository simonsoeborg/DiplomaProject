using System;
using System.Collections.Generic;

namespace ClassLibrary
{
    public class ProductItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Condition { get; set; } = null!;
        public string Quality { get; set; } = null!;
        public sbyte Sold { get; set; }
        public decimal? Weight { get; set; }
        public int Quantity { get; set; }
    }
}
