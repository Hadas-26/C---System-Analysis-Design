using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicaidEmploymentVerificationApplication.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationStatus_SeenByEmployeeAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Seen",
                table: "ApplicationStatuses",
                newName: "SeenByEmployee");

            migrationBuilder.AddColumn<bool>(
                name: "SeenByApplicant",
                table: "ApplicationStatuses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeenByApplicant",
                table: "ApplicationStatuses");

            migrationBuilder.RenameColumn(
                name: "SeenByEmployee",
                table: "ApplicationStatuses",
                newName: "Seen");
        }
    }
}
