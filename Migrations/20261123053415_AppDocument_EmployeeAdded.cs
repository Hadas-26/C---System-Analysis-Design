using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicaidEmploymentVerificationApplication.Migrations
{
    /// <inheritdoc />
    public partial class AppDocument_EmployeeAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmployeeId",
                table: "Documents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_EmployeeId",
                table: "Documents",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_AspNetUsers_EmployeeId",
                table: "Documents",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_AspNetUsers_EmployeeId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_EmployeeId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Documents");
        }
    }
}
