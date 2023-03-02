using System;
using System.Collections.Generic;

namespace ClassLibrary.EFModels
{
    public partial class Product
    {
        public Product()
        {
            ProductItems = new HashSet<ProductItem>();
        }

        public int Id { get; set; }
        public int SubCatId { get; set; }
        public string Name { get; set; } = null!;
        public int ModelNumber { get; set; }
        public string Material { get; set; } = null!;
        public string? Design { get; set; }
        public string? Dimension { get; set; }

        public virtual SubCategory SubCat { get; set; } = null!;
        public virtual ICollection<ProductItem> ProductItems { get; set; }
    }
}
