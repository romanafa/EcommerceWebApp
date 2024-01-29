using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EcommerceWebApp_API.Migrations
{
    /// <inheritdoc />
    public partial class SeedProductAndCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryTitle", "IsActive" },
                values: new object[,]
                {
                    { 1, "Accessories", true },
                    { 2, "Decorations", true },
                    { 3, "Magnets", true }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "Colour", "CreatedAt", "Description", "Image", "IsActive", "Price", "ProductTitle", "Size", "Stock" },
                values: new object[,]
                {
                    { 1, "Multi", new DateTime(2024, 1, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Handmade headband", "https://ecommerceproductsimages.blob.core.windows.net/images/IMG_8882.JPG", true, 200.0, "Headband - Easter", "Onesize", 10 },
                    { 2, "Multi", new DateTime(2024, 1, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Handmade headband", "https://ecommerceproductsimages.blob.core.windows.net/images/IMG_8884.JPG", true, 200.0, "Headband - Pinky", "Onesize", 10 },
                    { 3, "Multi", new DateTime(2024, 1, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Handmade headband", "https://ecommerceproductsimages.blob.core.windows.net/images/IMG_8886.JPG", true, 200.0, "Headband - Summer", "Onesize", 10 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3);
        }
    }
}
