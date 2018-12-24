using Microsoft.EntityFrameworkCore.Migrations;

namespace Momento.Data.Migrations
{
    public partial class fixes333 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CodeLines_Code_CodeId",
                table: "CodeLines");

            migrationBuilder.DropForeignKey(
                name: "FK_CodeLines_Notes_NoteId",
                table: "CodeLines");

            migrationBuilder.DropIndex(
                name: "IX_CodeLines_CodeId",
                table: "CodeLines");

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

            migrationBuilder.DropColumn(
                name: "CodeId",
                table: "CodeLines");

            migrationBuilder.AlterColumn<int>(
                name: "NoteId",
                table: "CodeLines",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_CodeLines_Notes_NoteId",
                table: "CodeLines",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CodeLines_Notes_NoteId",
                table: "CodeLines");

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

            migrationBuilder.AlterColumn<int>(
                name: "NoteId",
                table: "CodeLines",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "CodeId",
                table: "CodeLines",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.AddForeignKey(
                name: "FK_CodeLines_Code_CodeId",
                table: "CodeLines",
                column: "CodeId",
                principalTable: "Code",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CodeLines_Notes_NoteId",
                table: "CodeLines",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
