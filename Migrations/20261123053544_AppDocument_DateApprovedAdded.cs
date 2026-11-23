using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicaidEmploymentVerificationApplication.Migrations
{
    /// <inheritdoc />
    public partial class AppDocument_DateApprovedAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateApproved",
                table: "Documents",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateApproved",
                table: "Documents");
        }
    }
}
