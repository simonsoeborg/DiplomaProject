using Microsoft.EntityFrameworkCore;

namespace ClassLibrary
{
    public class GroenlundDbContext : DbContext
    {
        public GroenlundDbContext() { }

        public GroenlundDbContext(DbContextOptions<GroenlundDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<DiscountCode> DiscountCodes { get; set; } = null!;
        public DbSet<Image> Images { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<Payment> Payments { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<ProductItem> ProductItems { get; set; } = null!;
        public DbSet<PriceHistory> PriceHistories { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Subcategory> Subcategories { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            if (!optionsBuilder.IsConfigured)
            {
                string connString = "Server=db.uglyrage.com,1433;Database=GroenlundDB;User=gluser;Password=gl1234;TrustServerCertificate=True";
                optionsBuilder.UseSqlServer(connString);
            }
        }
    }
}
