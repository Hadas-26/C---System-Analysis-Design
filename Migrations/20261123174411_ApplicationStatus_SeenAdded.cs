using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicaidEmploymentVerificationApplication.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationStatus_SeenAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Seen",
                table: "ApplicationStatuses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Seen",
                table: "ApplicationStatuses");
        }
    }
}
