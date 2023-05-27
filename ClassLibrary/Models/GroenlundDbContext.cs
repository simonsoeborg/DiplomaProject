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
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<DiscountCode> DiscountCodes { get; set; } = null!;
        public DbSet<Subcategory> Subcategories { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<ProductItem> ProductItems { get; set; } = null!;
        public DbSet<Image> Images { get; set; } = null!;
        public DbSet<PriceHistory> PriceHistories { get; set; } = null!;
        public DbSet<Payment> Payments { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;

        // Views
        public DbSet<SalesSummary> SalesSummary { get; set; } = null!;
        public DbSet<Inventory> CategoryProductCount { get; set; } = null!;
        public DbSet<ProductItemDetails> ProductsWithWeight { get; set; } = null!;

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

            /* Enable logging */
            //optionsBuilder.EnableSensitiveDataLogging();
            //optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<SalesSummary>(
            eb =>
            {
                eb.HasNoKey();
                eb.ToView("SalesSummary");
                eb.Property(v => v.TotalSales).HasColumnName("TotalSales");
                eb.Property(v => v.TotalAmount).HasColumnName("TotalAmount");
            });

            modelBuilder.Entity<ProductItemDetails>(
            eb =>
            {
                eb.HasNoKey();
                eb.ToView("ProductsWithWeight");
                eb.Property(v => v.Name).HasColumnName("Name");
                eb.Property(v => v.Material).HasColumnName("Material");
                eb.Property(v => v.Weight).HasColumnName("Weight");
            });

            /* modelBuilder.Entity<OrderDetails>(
             eb =>
             {
                 eb.HasNoKey();
                 eb.ToView("OrderDetails");
                 eb.Property(v => v.Name).HasColumnName("Name");
                 eb.Property(v => v.Manufacturer).HasColumnName("Manufacturer");
                 eb.Property(v => v.PaymentId).HasColumnName("PaymentId");
                 eb.Property(v => v.PaymentStatus).HasColumnName("PaymentStatus");
                 eb.Property(v => v.DeliveryStatus).HasColumnName("DeliveryStatus");
                 eb.Property(v => v.DiscountCode).HasColumnName("DiscountCode");
                 eb.Property(v => v.Active).HasColumnName("Active");
                 eb.Property(v => v.CustomerId).HasColumnName("CustomerId");
                 eb.Property(v => v.ProductItemId).HasColumnName("ProductItemId");
                 eb.Property(v => v.ProductId).HasColumnName("ProductId");

             });*/

            modelBuilder.Entity<Inventory>(
            eb =>
            {
                eb.HasNoKey();
                eb.ToView("CategoryProductCount");
                eb.Property(v => v.Name).HasColumnName("Name");
                eb.Property(v => v.TotalProducts).HasColumnName("TotalProducts");
            });
        }
    }
}
