using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sda_online_2_csharp_backend_teamwork1.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProductIdFromCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "category",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_category_ProductId",
                table: "category",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_category_product_ProductId",
                table: "category",
                column: "ProductId",
                principalTable: "product",
                principalColumn: "product_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_category_product_ProductId",
                table: "category");

            migrationBuilder.DropIndex(
                name: "IX_category_ProductId",
                table: "category");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "category");
        }
    }
}
