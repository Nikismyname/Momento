using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Momento.Data.Migrations
{
    public partial class op42 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CodeNotesHashtags");

            migrationBuilder.DropTable(
                name: "CodesHashtags");

            migrationBuilder.DropTable(
                name: "CodeNotes");

            migrationBuilder.DropTable(
                name: "Hashtags");

            migrationBuilder.DropTable(
                name: "Code");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1b347d74-8167-4f4c-a4cc-2566c4344954");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a2c367c1-7aeb-4ffd-b2cf-67f247b9efb7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c3d5acca-0173-4ece-a256-8ba17d1e3be6");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c868d00d-faab-464d-a583-8eabeb245150", "53e2a628-ba6f-4c76-bbaf-a13b4e8bd102", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a1f17130-620a-4f39-a339-f27d78271f75", "faf2a001-45ad-488e-8f12-84e418790bd6", "Moderator", "MODERATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "15e7046d-5bd7-4955-8e7e-5f648808f8c0", "795a5927-89d3-410e-b6ae-1c55d380335d", "User", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "15e7046d-5bd7-4955-8e7e-5f648808f8c0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a1f17130-620a-4f39-a339-f27d78271f75");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c868d00d-faab-464d-a583-8eabeb245150");

            migrationBuilder.CreateTable(
                name: "Code",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    DirectoryId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastModifiedOn = table.Column<DateTime>(nullable: true),
                    LastViewdOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    TimesModified = table.Column<int>(nullable: false),
                    TimesViewd = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Code", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Code_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "CodeNotes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CodeSnippetId = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Formatting = table.Column<int>(nullable: false),
                    Hashtags = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastModifiedOn = table.Column<DateTime>(nullable: true),
                    LastViewdOn = table.Column<DateTime>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    TimesModified = table.Column<int>(nullable: false),
                    TimesViewd = table.Column<int>(nullable: false),
                    WordId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeNotes_Code_CodeSnippetId",
                        column: x => x.CodeSnippetId,
                        principalTable: "Code",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1b347d74-8167-4f4c-a4cc-2566c4344954", "0def96c5-94f2-40c1-86ce-2de584e2ba12", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c3d5acca-0173-4ece-a256-8ba17d1e3be6", "e6888ecc-9260-4d71-8789-76cf301dc507", "Moderator", "MODERATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a2c367c1-7aeb-4ffd-b2cf-67f247b9efb7", "4c8be457-b2ff-4951-a847-cd1768652ed8", "User", "USER" });

            migrationBuilder.CreateIndex(
                name: "IX_Code_UserId",
                table: "Code",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeNotes_CodeSnippetId",
                table: "CodeNotes",
                column: "CodeSnippetId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeNotesHashtags_HashtagId",
                table: "CodeNotesHashtags",
                column: "HashtagId");

            migrationBuilder.CreateIndex(
                name: "IX_CodesHashtags_HashtagId",
                table: "CodesHashtags",
                column: "HashtagId");

            migrationBuilder.CreateIndex(
                name: "IX_Hashtags_Name",
                table: "Hashtags",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");
        }
    }
}
