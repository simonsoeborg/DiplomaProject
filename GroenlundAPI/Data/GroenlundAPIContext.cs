using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GroenlundAPI.Models;

namespace GroenlundAPI.Data
{
    public class GroenlundAPIContext : DbContext
    {
        public GroenlundAPIContext (DbContextOptions<GroenlundAPIContext> options)
            : base(options)
        {
        }

        public DbSet<GroenlundAPI.Models.Test> Test { get; set; } = default!;
    }
}
