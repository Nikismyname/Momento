using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Momento.Data.Migrations
{
    public partial class aloala : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ListsTodo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true),
                    DirectoryId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    Desctiption = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListsTodo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListsTodo_Directories_DirectoryId",
                        column: x => x.DirectoryId,
                        principalTable: "Directories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ListsTodo_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ListToDoItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ListToDoId = table.Column<int>(nullable: false),
                    Content = table.Column<string>(maxLength: 40, nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListToDoItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListToDoItems_ListsTodo_ListToDoId",
                        column: x => x.ListToDoId,
                        principalTable: "ListsTodo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListsTodo_DirectoryId",
                table: "ListsTodo",
                column: "DirectoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ListsTodo_UserId",
                table: "ListsTodo",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ListToDoItems_ListToDoId",
                table: "ListToDoItems",
                column: "ListToDoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListToDoItems");

            migrationBuilder.DropTable(
                name: "ListsTodo");
        }
    }
}
