using Microsoft.EntityFrameworkCore.Migrations;

namespace Momento.Data.Migrations
{
    public partial class remove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentFormatted",
                table: "Points");

            migrationBuilder.DropColumn(
                name: "Preview",
                table: "Points");

            migrationBuilder.DropColumn(
                name: "PreviewFormatted",
                table: "Points");

            migrationBuilder.DropColumn(
                name: "FormattedContent",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "FormattedPreview",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "Preview",
                table: "Notes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentFormatted",
                table: "Points",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Preview",
                table: "Points",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviewFormatted",
                table: "Points",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FormattedContent",
                table: "Notes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FormattedPreview",
                table: "Notes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Preview",
                table: "Notes",
                nullable: true);
        }
    }
}
