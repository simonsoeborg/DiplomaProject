using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class ManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subcategories_Products_ProductId",
                table: "Subcategories");

            migrationBuilder.DropIndex(
                name: "IX_Subcategories_ProductId",
                table: "Subcategories");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Subcategories");

            migrationBuilder.CreateTable(
                name: "ProductSubcategory",
                columns: table => new
                {
                    ProductsId = table.Column<int>(type: "int", nullable: false),
                    SubcategoriesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSubcategory", x => new { x.ProductsId, x.SubcategoriesId });
                    table.ForeignKey(
                        name: "FK_ProductSubcategory_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductSubcategory_Subcategories_SubcategoriesId",
                        column: x => x.SubcategoriesId,
                        principalTable: "Subcategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSubcategory_SubcategoriesId",
                table: "ProductSubcategory",
                column: "SubcategoriesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductSubcategory");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Subcategories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subcategories_ProductId",
                table: "Subcategories",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subcategories_Products_ProductId",
                table: "Subcategories",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
