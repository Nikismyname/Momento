using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Momento.Data.Migrations
{
    public partial class codesnippets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "Contents");

            migrationBuilder.CreateTable(
                name: "CodeSnippets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DirectoryId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeSnippets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hashtags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hashtags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Videos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Url = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    SeekTo = table.Column<int>(nullable: true),
                    DirectoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Videos_Directories_DirectoryId",
                        column: x => x.DirectoryId,
                        principalTable: "Directories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CodeSnippetNotes",
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
                        name: "FK_CodeSnippetsHashtags_CodeSnippets_CodeSnippetId",
                        column: x => x.CodeSnippetId,
                        principalTable: "CodeSnippets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CodeSnippetsHashtags_Hashtags_HashtagId",
                        column: x => x.HashtagId,
                        principalTable: "Hashtags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VideoNotes",
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
                    VideoId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VideoNotes_VideoNotes_ParentNoteId",
                        column: x => x.ParentNoteId,
                        principalTable: "VideoNotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VideoNotes_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CodeSnippetNotes_CodeSnippetId",
                table: "CodeSnippetNotes",
                column: "CodeSnippetId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeSnippetNotes_ParentNoteId",
                table: "CodeSnippetNotes",
                column: "ParentNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeSnippetsHashtags_HashtagId",
                table: "CodeSnippetsHashtags",
                column: "HashtagId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoNotes_ParentNoteId",
                table: "VideoNotes",
                column: "ParentNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoNotes_VideoId",
                table: "VideoNotes",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_Videos_DirectoryId",
                table: "Videos",
                column: "DirectoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CodeSnippetNotes");

            migrationBuilder.DropTable(
                name: "CodeSnippetsHashtags");

            migrationBuilder.DropTable(
                name: "VideoNotes");

            migrationBuilder.DropTable(
                name: "CodeSnippets");

            migrationBuilder.DropTable(
                name: "Hashtags");

            migrationBuilder.DropTable(
                name: "Videos");

            migrationBuilder.CreateTable(
                name: "Contents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    DirectoryId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Order = table.Column<int>(nullable: false),
                    SeekTo = table.Column<int>(nullable: true),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contents_Directories_DirectoryId",
                        column: x => x.DirectoryId,
                        principalTable: "Directories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(nullable: true),
                    Formatting = table.Column<int>(nullable: false),
                    Level = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    ParentContentId = table.Column<int>(nullable: true),
                    ParentNoteId = table.Column<int>(nullable: true),
                    SeekTo = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notes_Contents_ParentContentId",
                        column: x => x.ParentContentId,
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notes_Notes_ParentNoteId",
                        column: x => x.ParentNoteId,
                        principalTable: "Notes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contents_DirectoryId",
                table: "Contents",
                column: "DirectoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_ParentContentId",
                table: "Notes",
                column: "ParentContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_ParentNoteId",
                table: "Notes",
                column: "ParentNoteId");
        }
    }
}
