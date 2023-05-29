﻿// <auto-generated />
using System;
using ClassLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ClassLibrary.Migrations
{
    [DbContext(typeof(GroenlundDbContext))]
    partial class GroenlundDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ClassLibrary.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("ClassLibrary.Models.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CountryCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Phone")
                        .HasColumnType("int");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("ClassLibrary.Models.DiscountCode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DiscountPercentage")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("DiscountCodes");
                });

            modelBuilder.Entity("ClassLibrary.Models.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ProductItemId")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ProductItemId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("ClassLibrary.Models.Inventory", b =>
                {
                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Name");

                    b.Property<int>("TotalProducts")
                        .HasColumnType("int")
                        .HasColumnName("TotalProducts");

                    b.ToTable((string)null);

                    b.ToView("CategoryProductCount", (string)null);
                });

            modelBuilder.Entity("ClassLibrary.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<string>("DeliveryMethod")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeliveryStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("DiscountCodeId")
                        .HasColumnType("int");

                    b.Property<string>("OrderStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PaymentId")
                        .HasColumnType("int");

                    b.Property<double>("TotalPrice")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("DiscountCodeId");

                    b.HasIndex("PaymentId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("ClassLibrary.Models.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<DateTime>("DatePaid")
                        .HasColumnType("datetime2");

                    b.Property<string>("Method")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TransactionId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("ClassLibrary.Models.PriceHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ChangeDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProductItemId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductItemId");

                    b.ToTable("PriceHistories");
                });

            modelBuilder.Entity("ClassLibrary.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Design")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Dimension")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Manufacturer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Material")
                        .HasColumnType("int");

                    b.Property<string>("ModelNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("ClassLibrary.Models.ProductItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Condition")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("CurrentPrice")
                        .HasColumnType("decimal(12,2)");

                    b.Property<string>("CustomText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<decimal>("PurchasePrice")
                        .HasColumnType("decimal(12,2)");

                    b.Property<int>("Quality")
                        .HasColumnType("int");

                    b.Property<short>("Sold")
                        .HasColumnType("smallint");

                    b.Property<DateTime?>("SoldDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("Weight")
                        .HasColumnType("decimal(12,2)");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductItems");
                });

            modelBuilder.Entity("ClassLibrary.Models.ProductItemDetails", b =>
                {
                    b.Property<int>("Material")
                        .HasColumnType("int")
                        .HasColumnName("Material");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Name");

                    b.Property<decimal>("Weight")
                        .HasColumnType("decimal(12,2)")
                        .HasColumnName("Weight");

                    b.ToTable((string)null);

                    b.ToView("ProductsWithWeight", (string)null);
                });

            modelBuilder.Entity("ClassLibrary.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("ClassLibrary.Models.SalesSummary", b =>
                {
                    b.Property<double>("TotalAmount")
                        .HasColumnType("float")
                        .HasColumnName("TotalAmount");

                    b.Property<int>("TotalSales")
                        .HasColumnType("int")
                        .HasColumnName("TotalSales");

                    b.ToTable((string)null);

                    b.ToView("SalesSummary", (string)null);
                });

            modelBuilder.Entity("ClassLibrary.Models.Subcategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Subcategories");
                });

            modelBuilder.Entity("ClassLibrary.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("Age")
                        .HasColumnType("int");

                    b.Property<string>("CreatedAt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("OrderProductItem", b =>
                {
                    b.Property<int>("OrdersId")
                        .HasColumnType("int");

                    b.Property<int>("ProductItemsId")
                        .HasColumnType("int");

                    b.HasKey("OrdersId", "ProductItemsId");

                    b.HasIndex("ProductItemsId");

                    b.ToTable("OrderProductItem");
                });

            modelBuilder.Entity("ProductSubcategory", b =>
                {
                    b.Property<int>("ProductsId")
                        .HasColumnType("int");

                    b.Property<int>("SubcategoriesId")
                        .HasColumnType("int");

                    b.HasKey("ProductsId", "SubcategoriesId");

                    b.HasIndex("SubcategoriesId");

                    b.ToTable("ProductSubcategory");
                });

            modelBuilder.Entity("ClassLibrary.Models.Image", b =>
                {
                    b.HasOne("ClassLibrary.Models.ProductItem", null)
                        .WithMany("Images")
                        .HasForeignKey("ProductItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ClassLibrary.Models.Order", b =>
                {
                    b.HasOne("ClassLibrary.Models.Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClassLibrary.Models.DiscountCode", "DiscountCode")
                        .WithMany()
                        .HasForeignKey("DiscountCodeId");

                    b.HasOne("ClassLibrary.Models.Payment", "Payment")
                        .WithMany()
                        .HasForeignKey("PaymentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("DiscountCode");

                    b.Navigation("Payment");
                });

            modelBuilder.Entity("ClassLibrary.Models.PriceHistory", b =>
                {
                    b.HasOne("ClassLibrary.Models.ProductItem", "ProductItem")
                        .WithMany("PriceHistories")
                        .HasForeignKey("ProductItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProductItem");
                });

            modelBuilder.Entity("ClassLibrary.Models.ProductItem", b =>
                {
                    b.HasOne("ClassLibrary.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("ClassLibrary.Models.Subcategory", b =>
                {
                    b.HasOne("ClassLibrary.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("ClassLibrary.Models.User", b =>
                {
                    b.HasOne("ClassLibrary.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("OrderProductItem", b =>
                {
                    b.HasOne("ClassLibrary.Models.Order", null)
                        .WithMany()
                        .HasForeignKey("OrdersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClassLibrary.Models.ProductItem", null)
                        .WithMany()
                        .HasForeignKey("ProductItemsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProductSubcategory", b =>
                {
                    b.HasOne("ClassLibrary.Models.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClassLibrary.Models.Subcategory", null)
                        .WithMany()
                        .HasForeignKey("SubcategoriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ClassLibrary.Models.Customer", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("ClassLibrary.Models.ProductItem", b =>
                {
                    b.Navigation("Images");

                    b.Navigation("PriceHistories");
                });
#pragma warning restore 612, 618
        }
    }
}
