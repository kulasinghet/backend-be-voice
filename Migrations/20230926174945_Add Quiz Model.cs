using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Be_My_Voice_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddQuizModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "quizModels",
                columns: table => new
                {
                    QuizID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuizName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuizDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuizType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuizVideo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuizAnswers = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quizModels", x => x.QuizID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "quizModels");
        }
    }
}
