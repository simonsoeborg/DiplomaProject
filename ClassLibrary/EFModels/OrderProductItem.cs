using System;
using System.Collections.Generic;

namespace ClassLibrary.EFModels
{
    public partial class OrderProductItem
    {
        public int ProductItemId { get; set; }
        public int OrderId { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual ProductItem ProductItem { get; set; } = null!;
    }
}
