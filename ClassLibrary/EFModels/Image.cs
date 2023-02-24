using System;
using System.Collections.Generic;

namespace ClassLibrary.EFModels
{
    public partial class Image
    {
        public int Id { get; set; }
        public string Url { get; set; } = null!;
    }
}
