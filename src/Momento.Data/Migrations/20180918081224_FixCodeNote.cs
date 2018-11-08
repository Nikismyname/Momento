using Microsoft.EntityFrameworkCore.Migrations;

namespace Momento.Data.Migrations
{
    public partial class FixCodeNote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CodeNotes_Code_CodeSnippetId",
                table: "CodeNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_CodeNotes_CodeNotes_ParentNoteId",
                table: "CodeNotes");

            migrationBuilder.DropIndex(
                name: "IX_CodeNotes_ParentNoteId",
                table: "CodeNotes");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "CodeNotes");

            migrationBuilder.DropColumn(
                name: "ParentNoteId",
                table: "CodeNotes");

            migrationBuilder.DropColumn(
                name: "SeekTo",
                table: "CodeNotes");

            migrationBuilder.AlterColumn<int>(
                name: "CodeSnippetId",
                table: "CodeNotes",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WordId",
                table: "CodeNotes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CodeNotesHashtags",
                columns: table => new
                {
                    CodeNoteId = table.Column<int>(nullable: false),
                    HashtagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeNotesHashtags", x => new { x.CodeNoteId, x.HashtagId });
                    table.ForeignKey(
                        name: "FK_CodeNotesHashtags_CodeNotes_CodeNoteId",
                        column: x => x.CodeNoteId,
                        principalTable: "CodeNotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CodeNotesHashtags_Hashtags_HashtagId",
                        column: x => x.HashtagId,
                        principalTable: "Hashtags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CodeNotesHashtags_HashtagId",
                table: "CodeNotesHashtags",
                column: "HashtagId");

            migrationBuilder.AddForeignKey(
                name: "FK_CodeNotes_Code_CodeSnippetId",
                table: "CodeNotes",
                column: "CodeSnippetId",
                principalTable: "Code",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CodeNotes_Code_CodeSnippetId",
                table: "CodeNotes");

            migrationBuilder.DropTable(
                name: "CodeNotesHashtags");

            migrationBuilder.DropColumn(
                name: "WordId",
                table: "CodeNotes");

            migrationBuilder.AlterColumn<int>(
                name: "CodeSnippetId",
                table: "CodeNotes",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CodeNotes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentNoteId",
                table: "CodeNotes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SeekTo",
                table: "CodeNotes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CodeNotes_ParentNoteId",
                table: "CodeNotes",
                column: "ParentNoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_CodeNotes_Code_CodeSnippetId",
                table: "CodeNotes",
                column: "CodeSnippetId",
                principalTable: "Code",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CodeNotes_CodeNotes_ParentNoteId",
                table: "CodeNotes",
                column: "ParentNoteId",
                principalTable: "CodeNotes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
