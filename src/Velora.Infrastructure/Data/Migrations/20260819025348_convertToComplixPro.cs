using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Velora.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class convertToComplixPro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                schema: "velora",
                table: "CartItems",
                newName: "UnitPrice_Amount");

            migrationBuilder.RenameColumn(
                name: "OrderAmount",
                schema: "velora",
                table: "Cancellations",
                newName: "OrderAmount_Amount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UnitPrice_Amount",
                schema: "velora",
                table: "CartItems",
                newName: "UnitPrice");

            migrationBuilder.RenameColumn(
                name: "OrderAmount_Amount",
                schema: "velora",
                table: "Cancellations",
                newName: "OrderAmount");
        }
    }
}
