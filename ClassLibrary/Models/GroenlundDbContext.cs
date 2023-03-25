using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
            IConfigurationRoot root = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true).Build();
            string connString = root["GroenlundDB"]!;
            optionsBuilder.UseMySql(connString, ServerVersion.AutoDetect(connString));

            // string connString = root["MSSQL"]!;
            //optionsBuilder.UseSqlServer("Server=db.uglyrage.com,1433;Database=GroenlundDB;User Id=gluser;Password=gl1234;");
        }
    }
}
