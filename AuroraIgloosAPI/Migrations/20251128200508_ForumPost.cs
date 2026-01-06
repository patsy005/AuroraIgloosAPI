using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuroraIgloosAPI.Migrations
{
    /// <inheritdoc />
    public partial class ForumPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumPost_Employee_IdEmployee",
                table: "ForumPost");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumPost_ForumCategory_CategoryId",
                table: "ForumPost");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ForumPost",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PostContent",
                table: "ForumPost",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdEmployee",
                table: "ForumPost",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "ForumPost",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "UpdateDate",
                table: "ForumPost",
                type: "date",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ForumPost_Employee_IdEmployee",
                table: "ForumPost",
                column: "IdEmployee",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ForumPost_ForumCategory_CategoryId",
                table: "ForumPost",
                column: "CategoryId",
                principalTable: "ForumCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumPost_Employee_IdEmployee",
                table: "ForumPost");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumPost_ForumCategory_CategoryId",
                table: "ForumPost");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "ForumPost");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ForumPost",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PostContent",
                table: "ForumPost",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "IdEmployee",
                table: "ForumPost",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "ForumPost",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumPost_Employee_IdEmployee",
                table: "ForumPost",
                column: "IdEmployee",
                principalTable: "Employee",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumPost_ForumCategory_CategoryId",
                table: "ForumPost",
                column: "CategoryId",
                principalTable: "ForumCategory",
                principalColumn: "Id");
        }
    }
}
