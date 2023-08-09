using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Be_My_Voice_Backend.Migrations
{
    /// <inheritdoc />
    public partial class addStatusToTheSessionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "startDtae",
                table: "sessions",
                newName: "startDate");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "sessions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "sessions");

            migrationBuilder.RenameColumn(
                name: "startDate",
                table: "sessions",
                newName: "startDtae");
        }
    }
}
