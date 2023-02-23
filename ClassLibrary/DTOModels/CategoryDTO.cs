﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.DTOModels
{
    public class CategoryDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int Order { get; set; }

        public string? ImageUrl { get; set; }

        public string? Description { get; set; }
    }
}
