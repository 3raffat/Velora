using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryService.Infrastructure.Data.Migrtions
{
    /// <inheritdoc />
    public partial class AddDriverIdToDeliveryAttempts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DeliveryAttempts_ShipmentId_AttemptedAt",
                table: "DeliveryAttempts");

            migrationBuilder.AddColumn<Guid>(
                name: "DriverId",
                table: "DeliveryAttempts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryAttempts_ShipmentId_DriverId_AttemptedAt",
                table: "DeliveryAttempts",
                columns: new[] { "ShipmentId", "DriverId", "AttemptedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DeliveryAttempts_ShipmentId_DriverId_AttemptedAt",
                table: "DeliveryAttempts");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "DeliveryAttempts");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryAttempts_ShipmentId_AttemptedAt",
                table: "DeliveryAttempts",
                columns: new[] { "ShipmentId", "AttemptedAt" });
        }
    }
}
