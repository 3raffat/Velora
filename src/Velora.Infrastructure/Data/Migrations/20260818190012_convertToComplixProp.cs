using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Velora.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class convertToComplixProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount",
                schema: "velora",
                table: "Refunds",
                newName: "Amount_Amount");

            migrationBuilder.RenameColumn(
                name: "Amount",
                schema: "velora",
                table: "Payments",
                newName: "Amount_Amount");

            migrationBuilder.RenameColumn(
                name: "ShippingCost",
                schema: "velora",
                table: "Orders",
                newName: "ShippingCost_Amount");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                schema: "velora",
                table: "OrderItems",
                newName: "UnitPrice_Amount");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                schema: "velora",
                table: "Customers",
                newName: "PhoneNumber_Value");

            migrationBuilder.RenameColumn(
                name: "LastName",
                schema: "velora",
                table: "Customers",
                newName: "LastName_Value");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                schema: "velora",
                table: "Customers",
                newName: "FirstName_Value");

            migrationBuilder.RenameColumn(
                name: "Discount",
                schema: "velora",
                table: "Coupons",
                newName: "Discount_Amount");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                schema: "velora",
                table: "Customers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(225)",
                oldMaxLength: 225,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount_Amount",
                schema: "velora",
                table: "Refunds",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "Amount_Amount",
                schema: "velora",
                table: "Payments",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "ShippingCost_Amount",
                schema: "velora",
                table: "Orders",
                newName: "ShippingCost");

            migrationBuilder.RenameColumn(
                name: "UnitPrice_Amount",
                schema: "velora",
                table: "OrderItems",
                newName: "UnitPrice");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber_Value",
                schema: "velora",
                table: "Customers",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "LastName_Value",
                schema: "velora",
                table: "Customers",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "FirstName_Value",
                schema: "velora",
                table: "Customers",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "Discount_Amount",
                schema: "velora",
                table: "Coupons",
                newName: "Discount");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                schema: "velora",
                table: "Customers",
                type: "nvarchar(225)",
                maxLength: 225,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
