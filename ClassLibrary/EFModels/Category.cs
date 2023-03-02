using System;
using System.Collections.Generic;

namespace ClassLibrary.EFModels
{
    public partial class Category
    {
        public Category()
        {
            SubCategories = new HashSet<SubCategory>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Order { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<SubCategory> SubCategories { get; set; }
    }
}
