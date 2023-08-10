using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Be_My_Voice_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddUserTypeToTranslationModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "userType",
                table: "translations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "userType",
                table: "translations");
        }
    }
}
