using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Velora.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderAddressSnapshots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Addresses_BillingAddressId",
                schema: "velora",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Addresses_ShippingAddressId",
                schema: "velora",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_BillingAddressId",
                schema: "velora",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ShippingAddressId",
                schema: "velora",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BillingAddressId",
                schema: "velora",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingAddressId",
                schema: "velora",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress_City",
                schema: "velora",
                table: "Orders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress_Country",
                schema: "velora",
                table: "Orders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress_FullName",
                schema: "velora",
                table: "Orders",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress_PhoneNumber",
                schema: "velora",
                table: "Orders",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress_Street",
                schema: "velora",
                table: "Orders",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_City",
                schema: "velora",
                table: "Orders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_Country",
                schema: "velora",
                table: "Orders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_FullName",
                schema: "velora",
                table: "Orders",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_PhoneNumber",
                schema: "velora",
                table: "Orders",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_Street",
                schema: "velora",
                table: "Orders",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillingAddress_City",
                schema: "velora",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BillingAddress_Country",
                schema: "velora",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BillingAddress_FullName",
                schema: "velora",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BillingAddress_PhoneNumber",
                schema: "velora",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BillingAddress_Street",
                schema: "velora",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_City",
                schema: "velora",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_Country",
                schema: "velora",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_FullName",
                schema: "velora",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_PhoneNumber",
                schema: "velora",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_Street",
                schema: "velora",
                table: "Orders");

            migrationBuilder.AddColumn<Guid>(
                name: "BillingAddressId",
                schema: "velora",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ShippingAddressId",
                schema: "velora",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BillingAddressId",
                schema: "velora",
                table: "Orders",
                column: "BillingAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShippingAddressId",
                schema: "velora",
                table: "Orders",
                column: "ShippingAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Addresses_BillingAddressId",
                schema: "velora",
                table: "Orders",
                column: "BillingAddressId",
                principalSchema: "velora",
                principalTable: "Addresses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Addresses_ShippingAddressId",
                schema: "velora",
                table: "Orders",
                column: "ShippingAddressId",
                principalSchema: "velora",
                principalTable: "Addresses",
                principalColumn: "Id");
        }
    }
}
