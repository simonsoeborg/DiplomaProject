using Microsoft.EntityFrameworkCore;

namespace ClassLibrary
{
    public class GroenlundDbContext : DbContext
    {
        public GroenlundDbContext()
        {
        }

        public GroenlundDbContext(DbContextOptions<GroenlundDbContext> options)
            : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<DiscountCode> DiscountCodes { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductItem> ProductItems { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<User> Users { get; set; }



        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var connectionString = "Server=130.225.170.249;Database=GroenlundDB;User=GroenlundDB;Password=gl12345;Trusted_Connection=True;";
        //    optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("server=130.225.170.249;database=GroenlundDb;user=GroenlundDB;password=gl12345", ServerVersion.Parse("5.7.33-mysql"));
            }
        }

    }
}
