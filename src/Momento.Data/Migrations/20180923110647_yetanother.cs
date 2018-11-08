using Microsoft.EntityFrameworkCore.Migrations;

namespace Momento.Data.Migrations
{
    public partial class yetanother : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CSSTheme",
                table: "UsersSettings");

            migrationBuilder.RenameColumn(
                name: "PauseVideoOnTopicTop",
                table: "UsersSettings",
                newName: "VNPauseVideoOnTopicTop");

            migrationBuilder.RenameColumn(
                name: "PauseVideoOnTopicBottom",
                table: "UsersSettings",
                newName: "VNPauseVideoOnTopicBottom");

            migrationBuilder.RenameColumn(
                name: "PauseVideoOnTopNewNote",
                table: "UsersSettings",
                newName: "VNPauseVideoOnTopNewNote");

            migrationBuilder.RenameColumn(
                name: "PauseVideoOnTimeStampTop",
                table: "UsersSettings",
                newName: "VNPauseVideoOnTimeStampTop");

            migrationBuilder.RenameColumn(
                name: "PauseVideoOnTimeStampBottom",
                table: "UsersSettings",
                newName: "VNPauseVideoOnTimeStampBottom");

            migrationBuilder.RenameColumn(
                name: "PauseVideoOnSubNoteTop",
                table: "UsersSettings",
                newName: "VNPauseVideoOnSubNoteTop");

            migrationBuilder.RenameColumn(
                name: "PauseVideoOnBottomNewNote",
                table: "UsersSettings",
                newName: "VNPauseVideoOnBottomNewNote");

            migrationBuilder.RenameColumn(
                name: "GoDownOnNewTopicTop",
                table: "UsersSettings",
                newName: "VNGoDownOnNewTopicTop");

            migrationBuilder.RenameColumn(
                name: "GoDownOnNewTimeStampTop",
                table: "UsersSettings",
                newName: "VNGoDownOnNewTimeStampTop");

            migrationBuilder.RenameColumn(
                name: "GoDownOnNewNoteTop",
                table: "UsersSettings",
                newName: "VNGoDownOnNewNoteTop");

            migrationBuilder.RenameColumn(
                name: "DarkInputs",
                table: "UsersSettings",
                newName: "LADarkInputs");

            migrationBuilder.AddColumn<int>(
                name: "LACSSTheme",
                table: "UsersSettings",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LACSSTheme",
                table: "UsersSettings");

            migrationBuilder.RenameColumn(
                name: "VNPauseVideoOnTopicTop",
                table: "UsersSettings",
                newName: "PauseVideoOnTopicTop");

            migrationBuilder.RenameColumn(
                name: "VNPauseVideoOnTopicBottom",
                table: "UsersSettings",
                newName: "PauseVideoOnTopicBottom");

            migrationBuilder.RenameColumn(
                name: "VNPauseVideoOnTopNewNote",
                table: "UsersSettings",
                newName: "PauseVideoOnTopNewNote");

            migrationBuilder.RenameColumn(
                name: "VNPauseVideoOnTimeStampTop",
                table: "UsersSettings",
                newName: "PauseVideoOnTimeStampTop");

            migrationBuilder.RenameColumn(
                name: "VNPauseVideoOnTimeStampBottom",
                table: "UsersSettings",
                newName: "PauseVideoOnTimeStampBottom");

            migrationBuilder.RenameColumn(
                name: "VNPauseVideoOnSubNoteTop",
                table: "UsersSettings",
                newName: "PauseVideoOnSubNoteTop");

            migrationBuilder.RenameColumn(
                name: "VNPauseVideoOnBottomNewNote",
                table: "UsersSettings",
                newName: "PauseVideoOnBottomNewNote");

            migrationBuilder.RenameColumn(
                name: "VNGoDownOnNewTopicTop",
                table: "UsersSettings",
                newName: "GoDownOnNewTopicTop");

            migrationBuilder.RenameColumn(
                name: "VNGoDownOnNewTimeStampTop",
                table: "UsersSettings",
                newName: "GoDownOnNewTimeStampTop");

            migrationBuilder.RenameColumn(
                name: "VNGoDownOnNewNoteTop",
                table: "UsersSettings",
                newName: "GoDownOnNewNoteTop");

            migrationBuilder.RenameColumn(
                name: "LADarkInputs",
                table: "UsersSettings",
                newName: "DarkInputs");

            migrationBuilder.AddColumn<int>(
                name: "CSSTheme",
                table: "UsersSettings",
                nullable: false,
                defaultValue: 0);
        }
    }
}
