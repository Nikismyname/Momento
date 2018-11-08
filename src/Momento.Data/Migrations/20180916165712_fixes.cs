using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Momento.Data.Migrations
{
    public partial class fixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CodeSnippetsHashtags_CodeSnippets_CodeSnippetId",
                table: "CodeSnippetsHashtags");

            migrationBuilder.DropTable(
                name: "CodeSnippetNotes");

            migrationBuilder.DropTable(
                name: "CodeSnippets");

            migrationBuilder.CreateTable(
                name: "Code",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DirectoryId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Content = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Code", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CodeNotes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    SeekTo = table.Column<int>(nullable: true),
                    Formatting = table.Column<int>(nullable: false),
                    Level = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    ParentNoteId = table.Column<int>(nullable: true),
                    CodeSnippetId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeNotes_Code_CodeSnippetId",
                        column: x => x.CodeSnippetId,
                        principalTable: "Code",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CodeNotes_CodeNotes_ParentNoteId",
                        column: x => x.ParentNoteId,
                        principalTable: "CodeNotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CodeNotes_CodeSnippetId",
                table: "CodeNotes",
                column: "CodeSnippetId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeNotes_ParentNoteId",
                table: "CodeNotes",
                column: "ParentNoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_CodeSnippetsHashtags_Code_CodeSnippetId",
                table: "CodeSnippetsHashtags",
                column: "CodeSnippetId",
                principalTable: "Code",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CodeSnippetsHashtags_Code_CodeSnippetId",
                table: "CodeSnippetsHashtags");

            migrationBuilder.DropTable(
                name: "CodeNotes");

            migrationBuilder.DropTable(
                name: "Code");

            migrationBuilder.CreateTable(
                name: "CodeSnippets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(nullable: true),
                    DirectoryId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeSnippets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CodeSnippetNotes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CodeSnippetId = table.Column<int>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Formatting = table.Column<int>(nullable: false),
                    Level = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    ParentNoteId = table.Column<int>(nullable: true),
                    SeekTo = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeSnippetNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeSnippetNotes_CodeSnippets_CodeSnippetId",
                        column: x => x.CodeSnippetId,
                        principalTable: "CodeSnippets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CodeSnippetNotes_CodeSnippetNotes_ParentNoteId",
                        column: x => x.ParentNoteId,
                        principalTable: "CodeSnippetNotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CodeSnippetNotes_CodeSnippetId",
                table: "CodeSnippetNotes",
                column: "CodeSnippetId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeSnippetNotes_ParentNoteId",
                table: "CodeSnippetNotes",
                column: "ParentNoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_CodeSnippetsHashtags_CodeSnippets_CodeSnippetId",
                table: "CodeSnippetsHashtags",
                column: "CodeSnippetId",
                principalTable: "CodeSnippets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
