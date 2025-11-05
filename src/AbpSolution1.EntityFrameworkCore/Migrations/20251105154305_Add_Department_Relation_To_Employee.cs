using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbpSolution1.Migrations
{
    /// <inheritdoc />
    public partial class Add_Department_Relation_To_Employee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Spin_Employees_DepartmentId",
                table: "Spin_Employees",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Spin_Employees_Spin_Departments_DepartmentId",
                table: "Spin_Employees",
                column: "DepartmentId",
                principalTable: "Spin_Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Spin_Employees_Spin_Departments_DepartmentId",
                table: "Spin_Employees");

            migrationBuilder.DropIndex(
                name: "IX_Spin_Employees_DepartmentId",
                table: "Spin_Employees");
        }
    }
}
