using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommerceWebApp_API.Migrations
{
    /// <inheritdoc />
    public partial class ColumnTypoFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryProduct_Products_ProductsProductionId",
                table: "CategoryProduct");

            migrationBuilder.RenameColumn(
                name: "ProductionId",
                table: "Products",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "ProductsProductionId",
                table: "CategoryProduct",
                newName: "ProductsProductId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryProduct_ProductsProductionId",
                table: "CategoryProduct",
                newName: "IX_CategoryProduct_ProductsProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryProduct_Products_ProductsProductId",
                table: "CategoryProduct",
                column: "ProductsProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryProduct_Products_ProductsProductId",
                table: "CategoryProduct");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Products",
                newName: "ProductionId");

            migrationBuilder.RenameColumn(
                name: "ProductsProductId",
                table: "CategoryProduct",
                newName: "ProductsProductionId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryProduct_ProductsProductId",
                table: "CategoryProduct",
                newName: "IX_CategoryProduct_ProductsProductionId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryProduct_Products_ProductsProductionId",
                table: "CategoryProduct",
                column: "ProductsProductionId",
                principalTable: "Products",
                principalColumn: "ProductionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
