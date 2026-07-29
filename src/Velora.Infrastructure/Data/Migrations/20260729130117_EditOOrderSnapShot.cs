using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Velora.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditOOrderSnapShot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillingAddress_City",
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
                name: "ShippingAddress_PhoneNumber",
                schema: "velora",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_Street",
                schema: "velora",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "ShippingAddress_Country",
                schema: "velora",
                table: "Orders",
                newName: "Country");

            migrationBuilder.RenameColumn(
                name: "ShippingAddress_City",
                schema: "velora",
                table: "Orders",
                newName: "City");

            migrationBuilder.RenameColumn(
                name: "ShippingAddress_FullName",
                schema: "velora",
                table: "Orders",
                newName: "AddressLine1");

            migrationBuilder.RenameColumn(
                name: "BillingAddress_Country",
                schema: "velora",
                table: "Orders",
                newName: "State");

            migrationBuilder.AddColumn<string>(
                name: "AddressLine2",
                schema: "velora",
                table: "Orders",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressLine2",
                schema: "velora",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "Country",
                schema: "velora",
                table: "Orders",
                newName: "ShippingAddress_Country");

            migrationBuilder.RenameColumn(
                name: "City",
                schema: "velora",
                table: "Orders",
                newName: "ShippingAddress_City");

            migrationBuilder.RenameColumn(
                name: "State",
                schema: "velora",
                table: "Orders",
                newName: "BillingAddress_Country");

            migrationBuilder.RenameColumn(
                name: "AddressLine1",
                schema: "velora",
                table: "Orders",
                newName: "ShippingAddress_FullName");

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress_City",
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
    }
}
