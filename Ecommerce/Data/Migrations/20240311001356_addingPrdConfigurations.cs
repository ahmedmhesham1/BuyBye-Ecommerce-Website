using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Data.Migrations
{
    /// <inheritdoc />
    public partial class addingPrdConfigurations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Addresses_AddressId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_VariationOptions_ProductItems_ProductItemId",
                table: "VariationOptions");

            migrationBuilder.DropIndex(
                name: "IX_VariationOptions_ProductItemId",
                table: "VariationOptions");

            migrationBuilder.DropColumn(
                name: "ProductItemId",
                table: "VariationOptions");

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ProductItemVariationOption",
                columns: table => new
                {
                    ConfigurationsId = table.Column<int>(type: "int", nullable: false),
                    ProductItemsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductItemVariationOption", x => new { x.ConfigurationsId, x.ProductItemsId });
                    table.ForeignKey(
                        name: "FK_ProductItemVariationOption_ProductItems_ProductItemsId",
                        column: x => x.ProductItemsId,
                        principalTable: "ProductItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductItemVariationOption_VariationOptions_ConfigurationsId",
                        column: x => x.ConfigurationsId,
                        principalTable: "VariationOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductItemVariationOption_ProductItemsId",
                table: "ProductItemVariationOption",
                column: "ProductItemsId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Addresses_AddressId",
                table: "User",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Addresses_AddressId",
                table: "User");

            migrationBuilder.DropTable(
                name: "ProductItemVariationOption");

            migrationBuilder.AddColumn<int>(
                name: "ProductItemId",
                table: "VariationOptions",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "User",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_VariationOptions_ProductItemId",
                table: "VariationOptions",
                column: "ProductItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Addresses_AddressId",
                table: "User",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VariationOptions_ProductItems_ProductItemId",
                table: "VariationOptions",
                column: "ProductItemId",
                principalTable: "ProductItems",
                principalColumn: "Id");
        }
    }
}
