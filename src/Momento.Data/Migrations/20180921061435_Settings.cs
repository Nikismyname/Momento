using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Momento.Data.Migrations
{
    public partial class Settings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserSettings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true),
                    CSSTheme = table.Column<int>(nullable: false),
                    DarkInputs = table.Column<bool>(nullable: false),
                    PauseVideoOnTopNewNote = table.Column<bool>(nullable: false),
                    PauseVideoOnBottomNewNote = table.Column<bool>(nullable: false),
                    PauseVideoOnTopSubNoteTop = table.Column<bool>(nullable: false),
                    PauseVideoOnTopicTop = table.Column<bool>(nullable: false),
                    PauseVideoOnTopicBottom = table.Column<bool>(nullable: false),
                    PauseVideoOnTimeStampTop = table.Column<bool>(nullable: false),
                    PauseVideoOnTimeStampBottom = table.Column<bool>(nullable: false),
                    GoDownOnNewNoteTop = table.Column<bool>(nullable: false),
                    GoDownOnNewTopicTop = table.Column<bool>(nullable: false),
                    GoDownOnNewTimeStampTop = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSettings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_UserId",
                table: "UserSettings",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSettings");
        }
    }
}
