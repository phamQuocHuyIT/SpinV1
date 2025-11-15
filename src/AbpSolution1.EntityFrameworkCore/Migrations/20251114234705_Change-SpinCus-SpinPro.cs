using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbpSolution1.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSpinCusSpinPro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "SpinProducts");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "SpinProducts");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "SpinProducts");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "SpinProducts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SpinProducts");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "SpinProducts");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "SpinProducts");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "SpinCustomers");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "SpinCustomers");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "SpinCustomers");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "SpinCustomers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SpinCustomers");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "SpinCustomers");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "SpinCustomers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "SpinProducts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "SpinProducts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "SpinProducts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "SpinProducts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SpinProducts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "SpinProducts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifierId",
                table: "SpinProducts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "SpinCustomers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "SpinCustomers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "SpinCustomers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "SpinCustomers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SpinCustomers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "SpinCustomers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifierId",
                table: "SpinCustomers",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
