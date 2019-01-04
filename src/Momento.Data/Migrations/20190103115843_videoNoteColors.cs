using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Momento.Data.Migrations
{
    public partial class videoNoteColors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Points");

            migrationBuilder.DropTable(
                name: "Topics");

            migrationBuilder.DropTable(
                name: "CheatSheets");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "240faefb-4827-4604-8d58-c2eabfa2ea4d", "faf305cf-5b62-4ba7-885a-646ebfa3dc6d" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "29a2e4da-aa5c-4b84-959c-4e2a753c9f87", "8250e893-7b4c-4483-8bc2-b6c64cb553ab" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "580a0b98-a093-4343-a878-a21060270243", "faf0fa23-fc1b-43b4-98e1-4351905feff2" });

            migrationBuilder.AddColumn<string>(
                name: "BackgroundColor",
                table: "VideoNotes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BorderColor",
                table: "VideoNotes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TextColor",
                table: "VideoNotes",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "90c726bd-a6c7-4be5-b320-1524fa8921d7", "0ee78f78-3278-486b-9f1c-91206aab6c42", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "05f56272-52f4-4c46-bb5a-5dfcdff2c55d", "c90bcf0c-07ea-4658-bb1e-52be3d7458eb", "Moderator", "MODERATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "37c6f6cf-55c1-4766-b537-0444e2dda1ad", "6bde995e-763f-4034-9368-48a5ff00212b", "User", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "05f56272-52f4-4c46-bb5a-5dfcdff2c55d", "c90bcf0c-07ea-4658-bb1e-52be3d7458eb" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "37c6f6cf-55c1-4766-b537-0444e2dda1ad", "6bde995e-763f-4034-9368-48a5ff00212b" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "90c726bd-a6c7-4be5-b320-1524fa8921d7", "0ee78f78-3278-486b-9f1c-91206aab6c42" });

            migrationBuilder.DropColumn(
                name: "BackgroundColor",
                table: "VideoNotes");

            migrationBuilder.DropColumn(
                name: "BorderColor",
                table: "VideoNotes");

            migrationBuilder.DropColumn(
                name: "TextColor",
                table: "VideoNotes");

            migrationBuilder.CreateTable(
                name: "CheatSheets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(maxLength: 300, nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastModifiedOn = table.Column<DateTime>(nullable: true),
                    LastViewdOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    TimesModified = table.Column<int>(nullable: false),
                    TimesViewd = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheatSheets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheatSheets_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CheatSheetId = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastModifiedOn = table.Column<DateTime>(nullable: true),
                    LastViewdOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    TimesModified = table.Column<int>(nullable: false),
                    TimesViewd = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Topics_CheatSheets_CheatSheetId",
                        column: x => x.CheatSheetId,
                        principalTable: "CheatSheets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Points",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Formatting = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastModifiedOn = table.Column<DateTime>(nullable: true),
                    LastViewdOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    ParentPointId = table.Column<int>(nullable: true),
                    TimesModified = table.Column<int>(nullable: false),
                    TimesViewd = table.Column<int>(nullable: false),
                    TopicId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Points", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Points_Points_ParentPointId",
                        column: x => x.ParentPointId,
                        principalTable: "Points",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Points_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "580a0b98-a093-4343-a878-a21060270243", "faf0fa23-fc1b-43b4-98e1-4351905feff2", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "29a2e4da-aa5c-4b84-959c-4e2a753c9f87", "8250e893-7b4c-4483-8bc2-b6c64cb553ab", "Moderator", "MODERATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "240faefb-4827-4604-8d58-c2eabfa2ea4d", "faf305cf-5b62-4ba7-885a-646ebfa3dc6d", "User", "USER" });

            migrationBuilder.CreateIndex(
                name: "IX_CheatSheets_UserId",
                table: "CheatSheets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Points_ParentPointId",
                table: "Points",
                column: "ParentPointId");

            migrationBuilder.CreateIndex(
                name: "IX_Points_TopicId",
                table: "Points",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_CheatSheetId",
                table: "Topics",
                column: "CheatSheetId");
        }
    }
}
