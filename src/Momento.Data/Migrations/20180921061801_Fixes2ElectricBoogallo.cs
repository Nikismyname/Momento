using Microsoft.EntityFrameworkCore.Migrations;

namespace Momento.Data.Migrations
{
    public partial class Fixes2ElectricBoogallo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSettings_AspNetUsers_UserId",
                table: "UserSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSettings",
                table: "UserSettings");

            migrationBuilder.RenameTable(
                name: "UserSettings",
                newName: "UsersSettings");

            migrationBuilder.RenameIndex(
                name: "IX_UserSettings_UserId",
                table: "UsersSettings",
                newName: "IX_UsersSettings_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersSettings",
                table: "UsersSettings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersSettings_AspNetUsers_UserId",
                table: "UsersSettings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersSettings_AspNetUsers_UserId",
                table: "UsersSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersSettings",
                table: "UsersSettings");

            migrationBuilder.RenameTable(
                name: "UsersSettings",
                newName: "UserSettings");

            migrationBuilder.RenameIndex(
                name: "IX_UsersSettings_UserId",
                table: "UserSettings",
                newName: "IX_UserSettings_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSettings",
                table: "UserSettings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSettings_AspNetUsers_UserId",
                table: "UserSettings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
