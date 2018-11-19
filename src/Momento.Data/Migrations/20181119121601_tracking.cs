using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Momento.Data.Migrations
{
    public partial class tracking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Videos",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastViewdOn",
                table: "Videos",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimesModified",
                table: "Videos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimesViewd",
                table: "Videos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "VideoNotes",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastViewdOn",
                table: "VideoNotes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimesModified",
                table: "VideoNotes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimesViewd",
                table: "VideoNotes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "UsersSettings",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastViewdOn",
                table: "UsersSettings",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimesModified",
                table: "UsersSettings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimesViewd",
                table: "UsersSettings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Topics",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastViewdOn",
                table: "Topics",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimesModified",
                table: "Topics",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimesViewd",
                table: "Topics",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Points",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastViewdOn",
                table: "Points",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimesModified",
                table: "Points",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimesViewd",
                table: "Points",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "ListToDoItems",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastViewdOn",
                table: "ListToDoItems",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimesModified",
                table: "ListToDoItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimesViewd",
                table: "ListToDoItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "ListsTodo",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastViewdOn",
                table: "ListsTodo",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimesModified",
                table: "ListsTodo",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimesViewd",
                table: "ListsTodo",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "ListsRemind",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastViewdOn",
                table: "ListsRemind",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimesModified",
                table: "ListsRemind",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimesViewd",
                table: "ListsRemind",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "ListRemindItems",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastViewdOn",
                table: "ListRemindItems",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimesModified",
                table: "ListRemindItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimesViewd",
                table: "ListRemindItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Directories",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastViewdOn",
                table: "Directories",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimesModified",
                table: "Directories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimesViewd",
                table: "Directories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "CodeNotes",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastViewdOn",
                table: "CodeNotes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimesModified",
                table: "CodeNotes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimesViewd",
                table: "CodeNotes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Code",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastViewdOn",
                table: "Code",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimesModified",
                table: "Code",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimesViewd",
                table: "Code",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "CheatSheets",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastViewdOn",
                table: "CheatSheets",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimesModified",
                table: "CheatSheets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimesViewd",
                table: "CheatSheets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastViewdOn",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimesModified",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimesViewd",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "LastViewdOn",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "TimesModified",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "TimesViewd",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "VideoNotes");

            migrationBuilder.DropColumn(
                name: "LastViewdOn",
                table: "VideoNotes");

            migrationBuilder.DropColumn(
                name: "TimesModified",
                table: "VideoNotes");

            migrationBuilder.DropColumn(
                name: "TimesViewd",
                table: "VideoNotes");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "UsersSettings");

            migrationBuilder.DropColumn(
                name: "LastViewdOn",
                table: "UsersSettings");

            migrationBuilder.DropColumn(
                name: "TimesModified",
                table: "UsersSettings");

            migrationBuilder.DropColumn(
                name: "TimesViewd",
                table: "UsersSettings");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "LastViewdOn",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "TimesModified",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "TimesViewd",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Points");

            migrationBuilder.DropColumn(
                name: "LastViewdOn",
                table: "Points");

            migrationBuilder.DropColumn(
                name: "TimesModified",
                table: "Points");

            migrationBuilder.DropColumn(
                name: "TimesViewd",
                table: "Points");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "ListToDoItems");

            migrationBuilder.DropColumn(
                name: "LastViewdOn",
                table: "ListToDoItems");

            migrationBuilder.DropColumn(
                name: "TimesModified",
                table: "ListToDoItems");

            migrationBuilder.DropColumn(
                name: "TimesViewd",
                table: "ListToDoItems");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "ListsTodo");

            migrationBuilder.DropColumn(
                name: "LastViewdOn",
                table: "ListsTodo");

            migrationBuilder.DropColumn(
                name: "TimesModified",
                table: "ListsTodo");

            migrationBuilder.DropColumn(
                name: "TimesViewd",
                table: "ListsTodo");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "ListsRemind");

            migrationBuilder.DropColumn(
                name: "LastViewdOn",
                table: "ListsRemind");

            migrationBuilder.DropColumn(
                name: "TimesModified",
                table: "ListsRemind");

            migrationBuilder.DropColumn(
                name: "TimesViewd",
                table: "ListsRemind");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "ListRemindItems");

            migrationBuilder.DropColumn(
                name: "LastViewdOn",
                table: "ListRemindItems");

            migrationBuilder.DropColumn(
                name: "TimesModified",
                table: "ListRemindItems");

            migrationBuilder.DropColumn(
                name: "TimesViewd",
                table: "ListRemindItems");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Directories");

            migrationBuilder.DropColumn(
                name: "LastViewdOn",
                table: "Directories");

            migrationBuilder.DropColumn(
                name: "TimesModified",
                table: "Directories");

            migrationBuilder.DropColumn(
                name: "TimesViewd",
                table: "Directories");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "CodeNotes");

            migrationBuilder.DropColumn(
                name: "LastViewdOn",
                table: "CodeNotes");

            migrationBuilder.DropColumn(
                name: "TimesModified",
                table: "CodeNotes");

            migrationBuilder.DropColumn(
                name: "TimesViewd",
                table: "CodeNotes");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Code");

            migrationBuilder.DropColumn(
                name: "LastViewdOn",
                table: "Code");

            migrationBuilder.DropColumn(
                name: "TimesModified",
                table: "Code");

            migrationBuilder.DropColumn(
                name: "TimesViewd",
                table: "Code");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "CheatSheets");

            migrationBuilder.DropColumn(
                name: "LastViewdOn",
                table: "CheatSheets");

            migrationBuilder.DropColumn(
                name: "TimesModified",
                table: "CheatSheets");

            migrationBuilder.DropColumn(
                name: "TimesViewd",
                table: "CheatSheets");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastViewdOn",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TimesModified",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TimesViewd",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

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
    }
}
