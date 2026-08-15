using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Velora.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderTotalAmounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                schema: "velora",
                table: "Orders",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalBaseAmount",
                schema: "velora",
                table: "Orders",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalDiscountAmount",
                schema: "velora",
                table: "Orders",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalAmount",
                schema: "velora",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TotalBaseAmount",
                schema: "velora",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TotalDiscountAmount",
                schema: "velora",
                table: "Orders");
        }
    }
}
