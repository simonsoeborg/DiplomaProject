using Microsoft.EntityFrameworkCore;

namespace ClassLibrary.Models
{
    public class GroenlundDbContext : DbContext
    {
        public GroenlundDbContext() { }

        public GroenlundDbContext(DbContextOptions<GroenlundDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Subcategory> Subcategories { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<ProductItem> ProductItems { get; set; } = null!;
        public DbSet<Image> Images { get; set; } = null!;
        public DbSet<PriceHistory> PriceHistories { get; set; } = null!;
        public DbSet<Payment> Payments { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            optionsBuilder.UseMySql("Server=130.225.170.249;Database=TestDB;User=GroenlundDB;Password=gl12345;",
                ServerVersion.AutoDetect("Server=130.225.170.249;Database=TestDB;User=GroenlundDB;Password=gl12345;"));
        }
    }
}
