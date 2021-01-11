using Microsoft.EntityFrameworkCore.Migrations;

namespace ShopProject.Migrations
{
    public partial class buildmodels3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductInCart_ShoppingCart_ShoppingCartId",
                table: "ProductInCart");

            migrationBuilder.DropColumn(
                name: "SoppingCartId",
                table: "ProductInCart");

            migrationBuilder.AlterColumn<int>(
                name: "ShoppingCartId",
                table: "ProductInCart",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInCart_ShoppingCart_ShoppingCartId",
                table: "ProductInCart",
                column: "ShoppingCartId",
                principalTable: "ShoppingCart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductInCart_ShoppingCart_ShoppingCartId",
                table: "ProductInCart");

            migrationBuilder.AlterColumn<int>(
                name: "ShoppingCartId",
                table: "ProductInCart",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "SoppingCartId",
                table: "ProductInCart",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInCart_ShoppingCart_ShoppingCartId",
                table: "ProductInCart",
                column: "ShoppingCartId",
                principalTable: "ShoppingCart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
