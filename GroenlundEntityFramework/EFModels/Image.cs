using System;
using System.Collections.Generic;

namespace GroenlundEntityFramework.Models;

public partial class Image
{
    public int Id { get; set; }

    public string Url { get; set; } = null!;
}
