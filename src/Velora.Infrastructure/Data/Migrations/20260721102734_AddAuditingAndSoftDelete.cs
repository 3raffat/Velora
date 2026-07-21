using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Velora.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditingAndSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                schema: "velora",
                table: "Categories",
                newName: "IsDeleted");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "velora",
                table: "Refunds",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                schema: "velora",
                table: "Refunds",
                type: "uniqueidentifier",
                nullable: true);

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
                name: "UpdatedAt",
                schema: "velora",
                table: "Refunds",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                schema: "velora",
                table: "Refunds",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "velora",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                schema: "velora",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "velora",
                table: "Products",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "velora",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "velora",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "velora",
                table: "Products",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                schema: "velora",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "velora",
                table: "Payments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                schema: "velora",
                table: "Payments",
                type: "uniqueidentifier",
                nullable: true);

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
                name: "UpdatedAt",
                schema: "velora",
                table: "Payments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                schema: "velora",
                table: "Payments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "velora",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                schema: "velora",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true);

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
                name: "UpdatedAt",
                schema: "velora",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                schema: "velora",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                schema: "velora",
                table: "Feedbacks",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                schema: "velora",
                table: "Feedbacks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "velora",
                table: "Feedbacks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "velora",
                table: "Feedbacks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "velora",
                table: "Feedbacks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                schema: "velora",
                table: "Feedbacks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "velora",
                table: "Customers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                schema: "velora",
                table: "Customers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "velora",
                table: "Customers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "velora",
                table: "Customers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "velora",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "velora",
                table: "Customers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                schema: "velora",
                table: "Customers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "velora",
                table: "Categories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                schema: "velora",
                table: "Categories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "velora",
                table: "Categories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "velora",
                table: "Categories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "velora",
                table: "Categories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                schema: "velora",
                table: "Categories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                schema: "velora",
                table: "Carts",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                schema: "velora",
                table: "Carts",
                type: "uniqueidentifier",
                nullable: true);

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

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                schema: "velora",
                table: "Carts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                schema: "velora",
                table: "CartItems",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                schema: "velora",
                table: "CartItems",
                type: "uniqueidentifier",
                nullable: true);

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

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                schema: "velora",
                table: "CartItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "velora",
                table: "Cancellations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                schema: "velora",
                table: "Cancellations",
                type: "uniqueidentifier",
                nullable: true);

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

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "velora",
                table: "Cancellations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                schema: "velora",
                table: "Cancellations",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "velora",
                table: "Refunds");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "velora",
                table: "Refunds");

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
                name: "UpdatedAt",
                schema: "velora",
                table: "Refunds");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "velora",
                table: "Refunds");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "velora",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "velora",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "velora",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "velora",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "velora",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "velora",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "velora",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "velora",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "velora",
                table: "Payments");

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
                name: "UpdatedAt",
                schema: "velora",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "velora",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "velora",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "velora",
                table: "Orders");

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
                name: "UpdatedAt",
                schema: "velora",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "velora",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "velora",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "velora",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "velora",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "velora",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "velora",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "velora",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "velora",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "velora",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "velora",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "velora",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "velora",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "velora",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "velora",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "velora",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "velora",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "velora",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "velora",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "velora",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "velora",
                table: "Carts");

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
                name: "UpdatedBy",
                schema: "velora",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "velora",
                table: "CartItems");

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
                name: "UpdatedBy",
                schema: "velora",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "velora",
                table: "Cancellations");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "velora",
                table: "Cancellations");

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

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "velora",
                table: "Cancellations");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "velora",
                table: "Cancellations");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                schema: "velora",
                table: "Categories",
                newName: "IsActive");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                schema: "velora",
                table: "Feedbacks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                schema: "velora",
                table: "Carts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                schema: "velora",
                table: "CartItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
