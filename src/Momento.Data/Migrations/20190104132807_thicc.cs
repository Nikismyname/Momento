using Microsoft.EntityFrameworkCore.Migrations;

namespace Momento.Data.Migrations
{
    public partial class thicc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "BorderThickness",
                table: "VideoNotes",
                nullable: false,
                defaultValue: 0);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "1b347d74-8167-4f4c-a4cc-2566c4344954", "0def96c5-94f2-40c1-86ce-2de584e2ba12" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "a2c367c1-7aeb-4ffd-b2cf-67f247b9efb7", "4c8be457-b2ff-4951-a847-cd1768652ed8" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "c3d5acca-0173-4ece-a256-8ba17d1e3be6", "e6888ecc-9260-4d71-8789-76cf301dc507" });

            migrationBuilder.DropColumn(
                name: "BorderThickness",
                table: "VideoNotes");

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
    }
}
