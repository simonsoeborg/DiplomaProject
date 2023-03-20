using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassLibrary.Migrations
{
    public partial class AddImageToProductItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DatePaid",
                table: "Payments",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductItemId",
                table: "Images",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Images_ProductItemId",
                table: "Images",
                column: "ProductItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_ProductItems_ProductItemId",
                table: "Images",
                column: "ProductItemId",
                principalTable: "ProductItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_ProductItems_ProductItemId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_ProductItemId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ProductItemId",
                table: "Images");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DatePaid",
                table: "Payments",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");
        }
    }
}
