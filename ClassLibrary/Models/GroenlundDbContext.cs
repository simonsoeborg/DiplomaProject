using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ClassLibrary.Models
{
    public class GroenlundDbContext : DbContext
    {
        readonly IConfigurationRoot configRoot = new ConfigurationBuilder().AddJsonFile("db_appsettings.json", false, true).Build();

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
        public DbSet<Inventory> CategoryProductCount { get; set; } = null!;
        public DbSet<SalesSummary> SalesSummary { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderDetails> OrderDetails { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString;

            /* MySql GroenlundDB */
            //connectionString = configRoot.GetConnectionString("GroenlundDB")!;
            //optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            /* MySql TestDB */
            //connectionString = configRoot.GetConnectionString("TestDB")!;
            //optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            /* MsSql GroenlundDB */
            connectionString = configRoot.GetConnectionString("MsSql")!;
            optionsBuilder.UseSqlServer(connectionString);

        }
    }
}
