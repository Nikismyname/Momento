using Microsoft.EntityFrameworkCore.Migrations;

namespace Momento.Data.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "066119c2-d4d2-4686-b10d-7c575626d2cd", "5e92f4c7-ca3b-41e6-bc54-0a91b2b87f7b", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "21c66e29-a031-4e59-9d12-df05edef7830", "0eed666e-7473-4df2-ae46-f6895c94e9ab", "Moderator", "MODERATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7c092bca-2980-440f-9af7-e9d28c352025", "482bab15-dc03-40f5-84aa-dae6fb92a899", "User", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "066119c2-d4d2-4686-b10d-7c575626d2cd", "5e92f4c7-ca3b-41e6-bc54-0a91b2b87f7b" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "21c66e29-a031-4e59-9d12-df05edef7830", "0eed666e-7473-4df2-ae46-f6895c94e9ab" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "7c092bca-2980-440f-9af7-e9d28c352025", "482bab15-dc03-40f5-84aa-dae6fb92a899" });
        }
    }
}
