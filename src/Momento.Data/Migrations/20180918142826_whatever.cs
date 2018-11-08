using Microsoft.EntityFrameworkCore.Migrations;

namespace Momento.Data.Migrations
{
    public partial class whatever : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Level",
                table: "CodeNotes");

            migrationBuilder.AlterColumn<int>(
                name: "VideoId",
                table: "VideoNotes",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Hashtags",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Code",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Hashtags_Name",
                table: "Hashtags",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Hashtags_Name",
                table: "Hashtags");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Code");

            migrationBuilder.AlterColumn<int>(
                name: "VideoId",
                table: "VideoNotes",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Hashtags",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "CodeNotes",
                nullable: false,
                defaultValue: 0);
        }
    }
}
