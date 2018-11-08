using Microsoft.EntityFrameworkCore.Migrations;

namespace Momento.Data.Migrations
{
    public partial class changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PauseVideoOnTopSubNoteTop",
                table: "UsersSettings",
                newName: "PauseVideoOnSubNoteTop");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PauseVideoOnSubNoteTop",
                table: "UsersSettings",
                newName: "PauseVideoOnTopSubNoteTop");
        }
    }
}
