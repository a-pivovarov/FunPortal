using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FunPortal.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCustomerConsolidateWithUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memberships_Customers_CustomerId",
                table: "Memberships");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Customers_CustomerId",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ShippingSlips_Customers_CustomerId",
                table: "ShippingSlips");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Customers_CustomerId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Users_CustomerId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "ShippingSlips",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ShippingSlips_CustomerId",
                table: "ShippingSlips",
                newName: "IX_ShippingSlips_UserId");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "PurchaseOrders",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseOrders_CustomerId",
                table: "PurchaseOrders",
                newName: "IX_PurchaseOrders_UserId");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Memberships",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Memberships_CustomerId",
                table: "Memberships",
                newName: "IX_Memberships_UserId");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Users",
                type: "TEXT",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Users",
                type: "TEXT",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Memberships_Users_UserId",
                table: "Memberships",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Users_UserId",
                table: "PurchaseOrders",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingSlips_Users_UserId",
                table: "ShippingSlips",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memberships_Users_UserId",
                table: "Memberships");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Users_UserId",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ShippingSlips_Users_UserId",
                table: "ShippingSlips");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ShippingSlips",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_ShippingSlips_UserId",
                table: "ShippingSlips",
                newName: "IX_ShippingSlips_CustomerId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "PurchaseOrders",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseOrders_UserId",
                table: "PurchaseOrders",
                newName: "IX_PurchaseOrders_CustomerId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Memberships",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Memberships_UserId",
                table: "Memberships",
                newName: "IX_Memberships_CustomerId");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Users",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Address = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_CustomerId",
                table: "Users",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Email",
                table: "Customers",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Memberships_Customers_CustomerId",
                table: "Memberships",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Customers_CustomerId",
                table: "PurchaseOrders",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingSlips_Customers_CustomerId",
                table: "ShippingSlips",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Customers_CustomerId",
                table: "Users",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
