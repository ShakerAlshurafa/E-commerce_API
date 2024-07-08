using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.Migrations
{
    public partial class addDataToTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Devices and gadgets", "Electronics" },
                    { 2, "Books and literature", "Books" },
                    { 3, "Apparel and accessories", "Clothing" }
                });

            migrationBuilder.InsertData(
                table: "localUsers",
                columns: new[] { "Id", "Email", "Name", "Password", "Phone", "Role" },
                values: new object[,]
                {
                    { 1, "Ahmad@gmail.com", "Ahmed Haggag", "password123", "0568752124", "Admin" },
                    { 2, "Tarek@gmail.com", "Tarek Sharim", "password456", "0598752324", "User" }
                });

            migrationBuilder.InsertData(
                table: "orders",
                columns: new[] { "Id", "LocalUserId", "OrderDate", "OrderStatus" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2023, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pending" },
                    { 2, 2, new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Completed" },
                    { 3, 1, new DateTime(2023, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shipped" }
                });

            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "Id", "CategoryId", "Image", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 1, "smartphone.jpg", "Smartphone", 299.99m },
                    { 2, 1, "laptop.jpg", "Laptop", 799.99m },
                    { 3, 2, "novel.jpg", "Novel", 19.99m },
                    { 4, 3, "tshirt.jpg", "T-Shirt", 9.99m },
                    { 5, 3, "jeans.jpg", "Jeans", 49.99m }
                });

            migrationBuilder.InsertData(
                table: "orderDetails",
                columns: new[] { "Id", "OrderId", "ProductId", "Price", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 1, 299.99m, 1m },
                    { 2, 1, 4, 9.99m, 2m },
                    { 3, 2, 3, 19.99m, 1m },
                    { 4, 3, 2, 799.99m, 1m },
                    { 5, 3, 5, 9.99m, 1m }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "orderDetails",
                keyColumns: new[] { "Id", "OrderId", "ProductId" },
                keyValues: new object[] { 1, 1, 1 });

            migrationBuilder.DeleteData(
                table: "orderDetails",
                keyColumns: new[] { "Id", "OrderId", "ProductId" },
                keyValues: new object[] { 2, 1, 4 });

            migrationBuilder.DeleteData(
                table: "orderDetails",
                keyColumns: new[] { "Id", "OrderId", "ProductId" },
                keyValues: new object[] { 3, 2, 3 });

            migrationBuilder.DeleteData(
                table: "orderDetails",
                keyColumns: new[] { "Id", "OrderId", "ProductId" },
                keyValues: new object[] { 4, 3, 2 });

            migrationBuilder.DeleteData(
                table: "orderDetails",
                keyColumns: new[] { "Id", "OrderId", "ProductId" },
                keyValues: new object[] { 5, 3, 5 });

            migrationBuilder.DeleteData(
                table: "orders",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "orders",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "orders",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "localUsers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "localUsers",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
