using System;
using System.Collections.Generic;

namespace ClassLibrary
{
    public class Product
    {
        public Product()
        {
        }

        public int Id { get; set; }
        public int SubCatId { get; set; }
        public string Name { get; set; } = null!;
        public int ModelNumber { get; set; }
        public string Material { get; set; } = null!;
        public string? Design { get; set; }
        public string? Dimension { get; set; }

    }
}
