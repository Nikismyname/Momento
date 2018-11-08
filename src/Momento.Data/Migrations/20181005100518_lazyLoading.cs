using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Momento.Data.Migrations
{
    public partial class lazyLoading : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoNotes_VideoNotes_ParentNoteId",
                table: "VideoNotes");

            migrationBuilder.RenameColumn(
                name: "ParentNoteId",
                table: "VideoNotes",
                newName: "NoteId");

            migrationBuilder.RenameIndex(
                name: "IX_VideoNotes_ParentNoteId",
                table: "VideoNotes",
                newName: "IX_VideoNotes_NoteId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Videos",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Videos",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Videos",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "Videos",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "VideoNotes",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "VideoNotes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "VideoNotes",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "UsersSettings",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UsersSettings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "UsersSettings",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Topics",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Topics",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "Topics",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Points",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Points",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "Points",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "ListToDoItems",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 40);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "ListToDoItems",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ListToDoItems",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "ListToDoItems",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "ListsTodo",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ListsTodo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "ListsTodo",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Lists",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Lists",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "Lists",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "ListItems",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 500);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "ListItems",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ListItems",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "ListItems",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Directories",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Directories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "Directories",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "CodeNotes",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CodeNotes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "CodeNotes",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Code",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Code",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "Code",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "CheatSheets",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CheatSheets",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "CheatSheets",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VideoNotes_VideoNotes_NoteId",
                table: "VideoNotes",
                column: "NoteId",
                principalTable: "VideoNotes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoNotes_VideoNotes_NoteId",
                table: "VideoNotes");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "VideoNotes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "VideoNotes");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "VideoNotes");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "UsersSettings");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UsersSettings");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "UsersSettings");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Points");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Points");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "Points");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "ListToDoItems");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ListToDoItems");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "ListToDoItems");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "ListsTodo");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ListsTodo");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "ListsTodo");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Lists");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Lists");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "Lists");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "ListItems");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ListItems");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "ListItems");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Directories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Directories");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "Directories");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "CodeNotes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CodeNotes");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "CodeNotes");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Code");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Code");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "Code");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "CheatSheets");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CheatSheets");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "CheatSheets");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "NoteId",
                table: "VideoNotes",
                newName: "ParentNoteId");

            migrationBuilder.RenameIndex(
                name: "IX_VideoNotes_NoteId",
                table: "VideoNotes",
                newName: "IX_VideoNotes_ParentNoteId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Videos",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "ListToDoItems",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "ListItems",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VideoNotes_VideoNotes_ParentNoteId",
                table: "VideoNotes",
                column: "ParentNoteId",
                principalTable: "VideoNotes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
