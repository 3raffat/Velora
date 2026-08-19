using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddShippingAddressSnapshot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingAddress",
                schema: "delivery",
                table: "Shipments");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_AddressLine1",
                schema: "delivery",
                table: "Shipments",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_AddressLine2",
                schema: "delivery",
                table: "Shipments",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_City",
                schema: "delivery",
                table: "Shipments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_Country",
                schema: "delivery",
                table: "Shipments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_State",
                schema: "delivery",
                table: "Shipments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingAddress_AddressLine1",
                schema: "delivery",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_AddressLine2",
                schema: "delivery",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_City",
                schema: "delivery",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_Country",
                schema: "delivery",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_State",
                schema: "delivery",
                table: "Shipments");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress",
                schema: "delivery",
                table: "Shipments",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }
    }
}
