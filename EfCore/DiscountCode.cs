using System;
using System.Collections.Generic;

namespace ClassLibrary
{
    public class DiscountCode
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public int DiscountPercentage { get; set; }
    }
}
