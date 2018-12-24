using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Momento.Data.Migrations
{
    public partial class notes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "3f0b7005-d559-45ce-8fb1-8b9c70305f87", "0f1f255c-70ca-4572-b6d5-c9cf0ff7f201" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "8646e35a-2fcf-4758-ba8b-a2da28449b87", "74dcda0d-0520-4567-afa3-9dc2424ad8b1" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "e7730a42-b033-4868-827a-d33ab862710e", "46e7b3ec-1b45-4287-b648-faf00c6d4d25" });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    LastModifiedOn = table.Column<DateTime>(nullable: true),
                    LastViewdOn = table.Column<DateTime>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    TimesModified = table.Column<int>(nullable: false),
                    TimesViewd = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    MainNoteContent = table.Column<string>(nullable: true),
                    EditorMode = table.Column<bool>(nullable: false),
                    Source = table.Column<string>(nullable: true),
                    ShowSourceEditor = table.Column<bool>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    DirectoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notes_Directories_DirectoryId",
                        column: x => x.DirectoryId,
                        principalTable: "Directories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CodeLines",
                columns: table => new
                {
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    LastModifiedOn = table.Column<DateTime>(nullable: true),
                    LastViewdOn = table.Column<DateTime>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    TimesModified = table.Column<int>(nullable: false),
                    TimesViewd = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SourceContent = table.Column<string>(nullable: true),
                    InPageId = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    NoteContent = table.Column<string>(nullable: true),
                    EditorMode = table.Column<bool>(nullable: false),
                    Visible = table.Column<bool>(nullable: false),
                    CodeId = table.Column<int>(nullable: false),
                    NoteId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeLines_Code_CodeId",
                        column: x => x.CodeId,
                        principalTable: "Code",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CodeLines_Notes_NoteId",
                        column: x => x.NoteId,
                        principalTable: "Notes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "084ab8a0-1c76-4462-b245-ca5be7c52160", "3ec6a1db-4d82-46a8-8234-8a3d04c35652", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5d94453d-82a0-4f55-81f2-fd0372af7c95", "6360a2ea-d4e7-4c20-81c9-bbd105016403", "Moderator", "MODERATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "fba77232-0c38-4a72-b71e-9978e3941531", "b1257ee8-3c50-4bb1-88f9-5e490a237fba", "User", "USER" });

            migrationBuilder.CreateIndex(
                name: "IX_CodeLines_CodeId",
                table: "CodeLines",
                column: "CodeId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeLines_NoteId",
                table: "CodeLines",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_DirectoryId",
                table: "Notes",
                column: "DirectoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_UserId",
                table: "Notes",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CodeLines");

            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "084ab8a0-1c76-4462-b245-ca5be7c52160", "3ec6a1db-4d82-46a8-8234-8a3d04c35652" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "5d94453d-82a0-4f55-81f2-fd0372af7c95", "6360a2ea-d4e7-4c20-81c9-bbd105016403" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "fba77232-0c38-4a72-b71e-9978e3941531", "b1257ee8-3c50-4bb1-88f9-5e490a237fba" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3f0b7005-d559-45ce-8fb1-8b9c70305f87", "0f1f255c-70ca-4572-b6d5-c9cf0ff7f201", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8646e35a-2fcf-4758-ba8b-a2da28449b87", "74dcda0d-0520-4567-afa3-9dc2424ad8b1", "Moderator", "MODERATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e7730a42-b033-4868-827a-d33ab862710e", "46e7b3ec-1b45-4287-b648-faf00c6d4d25", "User", "USER" });
        }
    }
}
