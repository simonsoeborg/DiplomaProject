using System;
using System.Collections.Generic;

namespace GroenlundEntityFramework.Models;

public partial class OrderDiscountCode
{
    public int OrderId { get; set; }

    public int DiscountCodeId { get; set; }

    public virtual DiscountCode DiscountCode { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
