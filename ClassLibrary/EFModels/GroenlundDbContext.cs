using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ClassLibrary.EFModels
{
    public partial class GroenlundDbContext : DbContext
    {
        public GroenlundDbContext()
        {
        }

        public GroenlundDbContext(DbContextOptions<GroenlundDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<DiscountCode> DiscountCodes { get; set; } = null!;
        public virtual DbSet<Image> Images { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDiscountCode> OrderDiscountCodes { get; set; } = null!;
        public virtual DbSet<OrderProductItem> OrderProductItems { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductImage> ProductImages { get; set; } = null!;
        public virtual DbSet<ProductItem> ProductItems { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<SubCategory> SubCategories { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=130.225.170.249;database=GroenlundDB;user=GroenlundDB;password=gl12345", Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.7.33-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8_general_ci")
                .HasCharSet("utf8");

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

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
                entity.ToTable("Customer");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever()
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
                entity.ToTable("Image");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Url)
                    .HasMaxLength(1000)
                    .HasColumnName("url");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.HasIndex(e => e.CustomerId, "customerID_idx");

                entity.HasIndex(e => e.PaymentId, "paymentID_idx");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Active)
                    .HasColumnType("tinyint(4)")
                    .HasColumnName("active")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.CustomerId)
                    .HasColumnType("int(11)")
                    .HasColumnName("customerID");

                entity.Property(e => e.DeliveryStatus)
                    .HasColumnType("enum('Not delivered','Sent','Delivered')")
                    .HasColumnName("deliveryStatus")
                    .HasDefaultValueSql("'Not delivered'");

                entity.Property(e => e.DiscountCode)
                    .HasMaxLength(255)
                    .HasColumnName("discountCode");

                entity.Property(e => e.PaymentId)
                    .HasColumnType("int(11)")
                    .HasColumnName("paymentID");

                entity.Property(e => e.PaymentStatus)
                    .HasColumnType("enum('Not payed','Payed online','Payed in shop')")
                    .HasColumnName("paymentStatus")
                    .HasDefaultValueSql("'Not payed'");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("customerID");

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.Orders)
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

                entity.HasOne(d => d.DiscountCode)
                    .WithMany()
                    .HasForeignKey(d => d.DiscountCodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_discount_id");

                entity.HasOne(d => d.Order)
                    .WithMany()
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

                entity.HasOne(d => d.Order)
                    .WithMany()
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("orderID");

                entity.HasOne(d => d.ProductItem)
                    .WithMany()
                    .HasForeignKey(d => d.ProductItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("productItemID");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
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
                entity.ToTable("Product");

                entity.HasIndex(e => e.SubCatId, "productSubcat_idx");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

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

                entity.Property(e => e.SubCatId)
                    .HasColumnType("int(11)")
                    .HasColumnName("subCatId");

                entity.HasOne(d => d.SubCat)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.SubCatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("productSubcat");
            });

            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ProductImage");

                entity.HasIndex(e => e.ImageId, "imageID");

                entity.HasIndex(e => e.ProductId, "productID_idx");

                entity.Property(e => e.ImageId)
                    .HasColumnType("int(11)")
                    .HasColumnName("imageID");

                entity.Property(e => e.ProductId)
                    .HasColumnType("int(11)")
                    .HasColumnName("productID");

                entity.HasOne(d => d.Image)
                    .WithMany()
                    .HasForeignKey(d => d.ImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("imageID");

                entity.HasOne(d => d.Product)
                    .WithMany()
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("productID");
            });

            modelBuilder.Entity<ProductItem>(entity =>
            {
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

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductItems)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_product_item_product");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleId)
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever()
                    .HasColumnName("roleId");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description");

                entity.Property(e => e.Title)
                    .HasMaxLength(45)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<SubCategory>(entity =>
            {
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

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.SubCategories)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("categoryID");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.RoleId, "userRoleId_idx");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Age)
                    .HasColumnType("int(11)")
                    .HasColumnName("age");

                entity.Property(e => e.CreatedAt)
                    .HasMaxLength(255)
                    .HasColumnName("created_at");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(45)
                    .HasColumnName("firstName");

                entity.Property(e => e.LastName)
                    .HasMaxLength(45)
                    .HasColumnName("lastName");

                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(255)
                    .HasColumnName("passwordHash");

                entity.Property(e => e.PasswordSalt)
                    .HasMaxLength(1000)
                    .HasColumnName("passwordSalt");

                entity.Property(e => e.RoleId)
                    .HasColumnType("int(11)")
                    .HasColumnName("roleId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("userRoleId");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
