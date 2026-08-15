using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Velora.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixOrderAddressColumnMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "State",
                schema: "velora",
                table: "Orders",
                newName: "ShippingState");

            migrationBuilder.RenameColumn(
                name: "Country",
                schema: "velora",
                table: "Orders",
                newName: "ShippingCountry");

            migrationBuilder.RenameColumn(
                name: "City",
                schema: "velora",
                table: "Orders",
                newName: "ShippingCity");

            migrationBuilder.RenameColumn(
                name: "AddressLine2",
                schema: "velora",
                table: "Orders",
                newName: "ShippingAddressLine2");

            migrationBuilder.RenameColumn(
                name: "AddressLine1",
                schema: "velora",
                table: "Orders",
                newName: "ShippingAddressLine1");

            migrationBuilder.AddColumn<string>(
                name: "BillingAddressLine1",
                schema: "velora",
                table: "Orders",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillingAddressLine2",
                schema: "velora",
                table: "Orders",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingCity",
                schema: "velora",
                table: "Orders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillingCountry",
                schema: "velora",
                table: "Orders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillingState",
                schema: "velora",
                table: "Orders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillingAddressLine1",
                schema: "velora",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BillingAddressLine2",
                schema: "velora",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BillingCity",
                schema: "velora",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BillingCountry",
                schema: "velora",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BillingState",
                schema: "velora",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "ShippingState",
                schema: "velora",
                table: "Orders",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "ShippingCountry",
                schema: "velora",
                table: "Orders",
                newName: "Country");

            migrationBuilder.RenameColumn(
                name: "ShippingCity",
                schema: "velora",
                table: "Orders",
                newName: "City");

            migrationBuilder.RenameColumn(
                name: "ShippingAddressLine2",
                schema: "velora",
                table: "Orders",
                newName: "AddressLine2");

            migrationBuilder.RenameColumn(
                name: "ShippingAddressLine1",
                schema: "velora",
                table: "Orders",
                newName: "AddressLine1");
        }
    }
}
