using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Be_My_Voice_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddNormalUSerTranslationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NormalUsertranslations",
                columns: table => new
                {
                    NormalUserTranslationID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NormalUserTranslatedText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NormalUsertranslations", x => x.NormalUserTranslationID);
                    table.ForeignKey(
                        name: "FK_NormalUsertranslations_sessions_SessionID",
                        column: x => x.SessionID,
                        principalTable: "sessions",
                        principalColumn: "sessionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NormalUsertranslations_SessionID",
                table: "NormalUsertranslations",
                column: "SessionID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NormalUsertranslations");
        }
    }
}
