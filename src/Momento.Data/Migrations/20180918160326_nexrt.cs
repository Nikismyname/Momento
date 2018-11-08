using Microsoft.EntityFrameworkCore.Migrations;

namespace Momento.Data.Migrations
{
    public partial class nexrt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CodeSnippetsHashtags");

            migrationBuilder.AddColumn<string>(
                name: "Hashtags",
                table: "CodeNotes",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CodesHashtags",
                columns: table => new
                {
                    CodeId = table.Column<int>(nullable: false),
                    HashtagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodesHashtags", x => new { x.CodeId, x.HashtagId });
                    table.ForeignKey(
                        name: "FK_CodesHashtags_Code_CodeId",
                        column: x => x.CodeId,
                        principalTable: "Code",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CodesHashtags_Hashtags_HashtagId",
                        column: x => x.HashtagId,
                        principalTable: "Hashtags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CodesHashtags_HashtagId",
                table: "CodesHashtags",
                column: "HashtagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CodesHashtags");

            migrationBuilder.DropColumn(
                name: "Hashtags",
                table: "CodeNotes");

            migrationBuilder.CreateTable(
                name: "CodeSnippetsHashtags",
                columns: table => new
                {
                    CodeSnippetId = table.Column<int>(nullable: false),
                    HashtagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeSnippetsHashtags", x => new { x.CodeSnippetId, x.HashtagId });
                    table.ForeignKey(
                        name: "FK_CodeSnippetsHashtags_Code_CodeSnippetId",
                        column: x => x.CodeSnippetId,
                        principalTable: "Code",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CodeSnippetsHashtags_Hashtags_HashtagId",
                        column: x => x.HashtagId,
                        principalTable: "Hashtags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CodeSnippetsHashtags_HashtagId",
                table: "CodeSnippetsHashtags",
                column: "HashtagId");
        }
    }
}
