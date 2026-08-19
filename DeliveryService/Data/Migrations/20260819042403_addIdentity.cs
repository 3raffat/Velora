using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryService.Data.Migrations
{
    /// <inheritdoc />
    public partial class addIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "delivery");

            migrationBuilder.RenameTable(
                name: "Shipments",
                newName: "Shipments",
                newSchema: "delivery");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Shipments",
                schema: "delivery",
                newName: "Shipments");
        }
    }
}
