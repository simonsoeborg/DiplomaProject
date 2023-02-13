using GroenlundJWT.Models;
using Microsoft.EntityFrameworkCore;

namespace GroenlundAPI.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public virtual DbSet<UserAuth>? UserAuths { get; set; }
    }
}
