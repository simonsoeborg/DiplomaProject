using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class LargerDecimalValueForProductItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Weight",
                table: "ProductItems",
                type: "decimal(12,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PurchasePrice",
                table: "ProductItems",
                type: "decimal(12,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentPrice",
                table: "ProductItems",
                type: "decimal(12,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Weight",
                table: "ProductItems",
                type: "decimal(6,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PurchasePrice",
                table: "ProductItems",
                type: "decimal(6,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentPrice",
                table: "ProductItems",
                type: "decimal(6,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)");
        }
    }
}
