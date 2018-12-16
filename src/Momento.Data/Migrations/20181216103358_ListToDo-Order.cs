using Microsoft.EntityFrameworkCore.Migrations;

namespace Momento.Data.Migrations
{
    public partial class ListToDoOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "ListsTodo",
                nullable: false,
                defaultValue: 0);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Order",
                table: "ListsTodo");

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
        }
    }
}
