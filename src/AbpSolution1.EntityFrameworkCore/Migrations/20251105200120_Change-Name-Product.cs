using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbpSolution1.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNameProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Pruducts",
                table: "Pruducts");

            migrationBuilder.RenameTable(
                name: "Pruducts",
                newName: "Spin_Products");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Spin_Products",
                table: "Spin_Products",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Spin_Products",
                table: "Spin_Products");

            migrationBuilder.RenameTable(
                name: "Spin_Products",
                newName: "Pruducts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pruducts",
                table: "Pruducts",
                column: "Id");
        }
    }
}
