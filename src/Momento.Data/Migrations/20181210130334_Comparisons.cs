using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Momento.Data.Migrations
{
    public partial class Comparisons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "10666100-56ed-4bc9-9ac0-1b6c6a30648e", "8ac60a02-7dd0-4637-a314-516b2209c63b" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "a6b53a5a-5f05-4f25-848f-bc4419449e55", "70ef60e6-8fcc-4f70-a1a3-5922482f8650" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "ca9cc3ee-7b84-4911-8aea-34a6454d1db7", "826a6ed4-bccb-432f-afd7-7c36db104a3a" });

            migrationBuilder.CreateTable(
                name: "Comparisons",
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
                    DirectoryId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    SourceLanguage = table.Column<string>(nullable: true),
                    TargetLanguage = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comparisons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comparisons_Directories_DirectoryId",
                        column: x => x.DirectoryId,
                        principalTable: "Directories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comparisons_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ComparisonItems",
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
                    Source = table.Column<string>(nullable: true),
                    Target = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    ComparisonId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComparisonItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComparisonItems_Comparisons_ComparisonId",
                        column: x => x.ComparisonId,
                        principalTable: "Comparisons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6d6cbb04-a886-41aa-a626-ae0d213e2f48", "3c8539d8-0ae9-48e1-94df-aa50009ecf74", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "93eaadb3-1b37-4c22-9183-b7799607739f", "7108f569-289d-46c0-9456-10a50468c622", "Moderator", "MODERATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ccff8a6c-2f54-42e9-8103-96e3f5137ce9", "1aed025d-e478-4398-9c7f-a99e27d8be94", "User", "USER" });

            migrationBuilder.CreateIndex(
                name: "IX_ComparisonItems_ComparisonId",
                table: "ComparisonItems",
                column: "ComparisonId");

            migrationBuilder.CreateIndex(
                name: "IX_Comparisons_DirectoryId",
                table: "Comparisons",
                column: "DirectoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Comparisons_UserId",
                table: "Comparisons",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComparisonItems");

            migrationBuilder.DropTable(
                name: "Comparisons");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "6d6cbb04-a886-41aa-a626-ae0d213e2f48", "3c8539d8-0ae9-48e1-94df-aa50009ecf74" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "93eaadb3-1b37-4c22-9183-b7799607739f", "7108f569-289d-46c0-9456-10a50468c622" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "ccff8a6c-2f54-42e9-8103-96e3f5137ce9", "1aed025d-e478-4398-9c7f-a99e27d8be94" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a6b53a5a-5f05-4f25-848f-bc4419449e55", "70ef60e6-8fcc-4f70-a1a3-5922482f8650", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "10666100-56ed-4bc9-9ac0-1b6c6a30648e", "8ac60a02-7dd0-4637-a314-516b2209c63b", "Moderator", "MODERATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ca9cc3ee-7b84-4911-8aea-34a6454d1db7", "826a6ed4-bccb-432f-afd7-7c36db104a3a", "User", "USER" });
        }
    }
}
