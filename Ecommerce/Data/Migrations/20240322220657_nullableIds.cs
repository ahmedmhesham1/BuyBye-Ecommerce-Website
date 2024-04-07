using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Data.Migrations
{
    /// <inheritdoc />
    public partial class nullableIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdersItems_ProductItems_ProductId",
                table: "OrdersItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductItems_Products_ProductId",
                table: "ProductItems");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ProductItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "OrdersItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersItems_ProductItems_ProductId",
                table: "OrdersItems",
                column: "ProductId",
                principalTable: "ProductItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductItems_Products_ProductId",
                table: "ProductItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdersItems_ProductItems_ProductId",
                table: "OrdersItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductItems_Products_ProductId",
                table: "ProductItems");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ProductItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "OrdersItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersItems_ProductItems_ProductId",
                table: "OrdersItems",
                column: "ProductId",
                principalTable: "ProductItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductItems_Products_ProductId",
                table: "ProductItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
