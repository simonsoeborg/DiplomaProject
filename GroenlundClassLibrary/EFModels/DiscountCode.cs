using System;
using System.Collections.Generic;

namespace GroenlundEntityFramework.Models;

public partial class DiscountCode
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public int DiscountPercentage { get; set; }
}
