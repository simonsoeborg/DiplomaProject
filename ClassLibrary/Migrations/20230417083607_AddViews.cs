using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AddViews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    PaymentId = table.Column<int>(type: "int", nullable: false),
                    ProductItemId = table.Column<int>(type: "int", nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliveryStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscountCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.Sql(@"CREATE VIEW ProductsWithWeight AS
                                SELECT p.Name, p.Material, pi.Weight
                                FROM Products p
                                INNER JOIN ProductItems pi ON p.Id = pi.ProductId
                                WHERE p.Material IN (4, 5) AND pi.Weight <> 0;");

            migrationBuilder.Sql(@"CREATE VIEW SalesSummary
                                AS
                                SELECT COUNT(*) AS TotalSales, SUM(Amount) AS TotalAmount
                                FROM [GroenlundDB].[dbo].[Payments]");

            migrationBuilder.Sql(@"CREATE VIEW OrderDetails
                                AS
                                SELECT o.Id AS OrderId, o.CustomerId, o.PaymentId, o.ProductItemId, o.PaymentStatus, o.DeliveryStatus, o.DiscountCode, o.Active, pi.ProductId, p.Name, p.Manufacturer
                                FROM Orders o
                                INNER JOIN ProductItems pi ON o.ProductItemId = pi.Id
                                INNER JOIN Products p ON pi.ProductId = p.Id");

            migrationBuilder.Sql(@"CREATE VIEW CategoryProductCount AS
                                SELECT c.Name, COUNT(p.Id) AS TotalProducts
                                FROM Categories c
                                LEFT JOIN SubCategories sc ON sc.CategoryID = c.Id
                                LEFT JOIN ProductSubcategory psc ON psc.SubcategoriesId = sc.Id
                                LEFT JOIN Products p ON p.Id = psc.ProductsId
                                GROUP BY c.Name;");



        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryProductCount");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "ProductsWithWeight");

            migrationBuilder.DropTable(
                name: "SalesSummary");
        }
    }
}
