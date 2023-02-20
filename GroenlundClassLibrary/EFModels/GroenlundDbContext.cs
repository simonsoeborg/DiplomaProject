using Microsoft.EntityFrameworkCore;

namespace GroenlundEntityFramework.Models;

public partial class GroenlundDbContext : DbContext
{
    public GroenlundDbContext()
    {
    }

    public GroenlundDbContext(DbContextOptions<GroenlundDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<DiscountCode> DiscountCodes { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDiscountCode> OrderDiscountCodes { get; set; }

    public virtual DbSet<OrderProductItem> OrderProductItems { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<ProductItem> ProductItems { get; set; }

    public virtual DbSet<SubCategory> SubCategories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql("Server=130.225.170.249;Database=GroenlundDB;User=GroenlundDB;Password=gl12345", ServerVersion.Parse("5.7.33-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8_general_ci")
            .HasCharSet("utf8");

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Category");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Categorycol)
                .HasMaxLength(45)
                .HasColumnName("categorycol");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("imageURL");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Order)
                .HasColumnType("int(11)")
                .HasColumnName("order");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Customer");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.Phone)
                .HasColumnType("int(11)")
                .HasColumnName("phone");
        });

        modelBuilder.Entity<DiscountCode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Discount_Codes");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.DiscountPercentage)
                .HasColumnType("int(11)")
                .HasColumnName("discountPercentage");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Image");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Url)
                .HasMaxLength(1000)
                .HasColumnName("url");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Order");

            entity.HasIndex(e => e.CustomerId, "customerID_idx");

            entity.HasIndex(e => e.PaymentId, "paymentID_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("'1'")
                .HasColumnType("tinyint(4)")
                .HasColumnName("active");
            entity.Property(e => e.CustomerId)
                .HasColumnType("int(11)")
                .HasColumnName("customerID");
            entity.Property(e => e.DeliveryStatus)
                .HasDefaultValueSql("'Not delivered'")
                .HasColumnType("enum('Not delivered','Sent','Delivered')")
                .HasColumnName("deliveryStatus");
            entity.Property(e => e.DiscountCode)
                .HasMaxLength(255)
                .HasColumnName("discountCode");
            entity.Property(e => e.PaymentId)
                .HasColumnType("int(11)")
                .HasColumnName("paymentID");
            entity.Property(e => e.PaymentStatus)
                .HasDefaultValueSql("'Not payed'")
                .HasColumnType("enum('Not payed','Payed online','Payed in shop')")
                .HasColumnName("paymentStatus");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("customerID");

            entity.HasOne(d => d.Payment).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PaymentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("paymentID");
        });

        modelBuilder.Entity<OrderDiscountCode>(entity =>
        {
            entity.HasNoKey();

            entity.HasIndex(e => e.DiscountCodeId, "fk_discount_id_idx");

            entity.HasIndex(e => e.OrderId, "fk_order_id");

            entity.Property(e => e.DiscountCodeId)
                .HasColumnType("int(11)")
                .HasColumnName("discountCodeID");
            entity.Property(e => e.OrderId)
                .HasColumnType("int(11)")
                .HasColumnName("orderID");

            entity.HasOne(d => d.DiscountCode).WithMany()
                .HasForeignKey(d => d.DiscountCodeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_discount_id");

            entity.HasOne(d => d.Order).WithMany()
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_id");
        });

        modelBuilder.Entity<OrderProductItem>(entity =>
        {
            entity.HasNoKey();

            entity.HasIndex(e => e.OrderId, "orderID_idx");

            entity.HasIndex(e => e.ProductItemId, "productItemID");

            entity.Property(e => e.OrderId)
                .HasColumnType("int(11)")
                .HasColumnName("orderID");
            entity.Property(e => e.ProductItemId)
                .HasColumnType("int(11)")
                .HasColumnName("productItemID");

            entity.HasOne(d => d.Order).WithMany()
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orderID");

            entity.HasOne(d => d.ProductItem).WithMany()
                .HasForeignKey(d => d.ProductItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("productItemID");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Payment");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Approved)
                .HasColumnType("tinyint(4)")
                .HasColumnName("approved");
            entity.Property(e => e.DatePaid).HasColumnName("datePaid");
            entity.Property(e => e.Method)
                .HasColumnType("enum('Creditcard','MobilePay','Stripe')")
                .HasColumnName("method");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Product");

            entity.HasIndex(e => e.Category, "category_idx");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Category)
                .HasColumnType("int(11)")
                .HasColumnName("category");
            entity.Property(e => e.Design)
                .HasMaxLength(500)
                .HasColumnName("design");
            entity.Property(e => e.Dimension)
                .HasMaxLength(255)
                .HasColumnName("dimension");
            entity.Property(e => e.Material)
                .HasColumnType("enum('porcelain','steel','gold','silver','glass')")
                .HasColumnName("material");
            entity.Property(e => e.ModelNumber)
                .HasColumnType("int(11)")
                .HasColumnName("modelNumber");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");

            entity.HasOne(d => d.CategoryNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.Category)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("category");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ProductImage");

            entity.HasIndex(e => e.ImageId, "imageID");

            entity.HasIndex(e => e.ProductId, "productID_idx");

            entity.Property(e => e.ImageId)
                .HasColumnType("int(11)")
                .HasColumnName("imageID");
            entity.Property(e => e.ProductId)
                .HasColumnType("int(11)")
                .HasColumnName("productID");

            entity.HasOne(d => d.Image).WithMany()
                .HasForeignKey(d => d.ImageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("imageID");

            entity.HasOne(d => d.Product).WithMany()
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("productID");
        });

        modelBuilder.Entity<ProductItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Product_Item");

            entity.HasIndex(e => e.ProductId, "fk_product_item_product_idx");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Condition)
                .HasColumnType("enum('No shards','Few shards','Many shards')")
                .HasColumnName("condition");
            entity.Property(e => e.ProductId)
                .HasColumnType("int(11)")
                .HasColumnName("product_id");
            entity.Property(e => e.Quality)
                .HasColumnType("enum('1.Quality','2.Quality','3.Quality')")
                .HasColumnName("quality");
            entity.Property(e => e.Quantity)
                .HasColumnType("int(11)")
                .HasColumnName("quantity");
            entity.Property(e => e.Sku)
                .HasColumnType("int(11)")
                .HasColumnName("sku");
            entity.Property(e => e.Sold)
                .HasColumnType("tinyint(4)")
                .HasColumnName("sold");
            entity.Property(e => e.Weight)
                .HasPrecision(4)
                .HasColumnName("weight");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_product_item_product");
        });

        modelBuilder.Entity<SubCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Sub_Category");

            entity.HasIndex(e => e.CategoryId, "categoryID_idx");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.CategoryId)
                .HasColumnType("int(11)")
                .HasColumnName("categoryID");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("imageURL");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Order)
                .HasColumnType("int(11)")
                .HasColumnName("order");

            entity.HasOne(d => d.Category).WithMany(p => p.SubCategories)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("categoryID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
