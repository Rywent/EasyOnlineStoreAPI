using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyOnlineStore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDbModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Warehouses_WarehouseId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Products",
                newName: "Stock");

            migrationBuilder.AddColumn<string>(
                name: "Adress",
                table: "Warehouses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Warehouses",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "DeliveryCost",
                table: "Warehouses",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Warehouses",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LikesCount",
                table: "Warehouses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Warehouses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Products",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "OldPrice",
                table: "Products",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Rating",
                table: "Products",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "SKU",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Products",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductImage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImage_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductImage_ProductId",
                table: "ProductImage",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Warehouses_WarehouseId",
                table: "Products",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Warehouses_WarehouseId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "ProductImage");

            migrationBuilder.DropColumn(
                name: "Adress",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "DeliveryCost",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "LikesCount",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OldPrice",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SKU",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Stock",
                table: "Products",
                newName: "Quantity");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Warehouses_WarehouseId",
                table: "Products",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
