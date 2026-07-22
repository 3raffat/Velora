using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Velora.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "velora",
                table: "Refunds");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "velora",
                table: "Refunds");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "velora",
                table: "Refunds");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "velora",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "velora",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "velora",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "velora",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "velora",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "velora",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "velora",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "velora",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "velora",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "velora",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "velora",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "velora",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "velora",
                table: "Cancellations");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "velora",
                table: "Cancellations");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "velora",
                table: "Cancellations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "velora",
                table: "Refunds",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "velora",
                table: "Refunds",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "velora",
                table: "Refunds",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "velora",
                table: "Payments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "velora",
                table: "Payments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "velora",
                table: "Payments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "velora",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "velora",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "velora",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "velora",
                table: "Carts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "velora",
                table: "Carts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "velora",
                table: "Carts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "velora",
                table: "CartItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "velora",
                table: "CartItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "velora",
                table: "CartItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "velora",
                table: "Cancellations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "velora",
                table: "Cancellations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "velora",
                table: "Cancellations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
