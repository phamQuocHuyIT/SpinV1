using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbpSolution1.Migrations
{
    /// <inheritdoc />
    public partial class AddRankedandTotalinCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Ranked",
                table: "Spin_Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "TotalPurchase",
                table: "Spin_Customers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ranked",
                table: "Spin_Customers");

            migrationBuilder.DropColumn(
                name: "TotalPurchase",
                table: "Spin_Customers");
        }
    }
}
