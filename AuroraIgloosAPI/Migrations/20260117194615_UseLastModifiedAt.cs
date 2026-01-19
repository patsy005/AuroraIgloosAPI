using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuroraIgloosAPI.Migrations
{
    /// <inheritdoc />
    public partial class UseLastModifiedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "UserType");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "UserType");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "UserRole");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "UserRole");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "TripSeason");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "TripLevelOfDifficulty");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Trip");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PaymentMethod");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "PaymentMethod");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Igloo");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Igloo");

            migrationBuilder.DropColumn(
                name: "PostDate",
                table: "ForumPost");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "ForumPost");

            migrationBuilder.DropColumn(
                name: "CommentDate",
                table: "ForumComment");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ForumComment");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ForumCategory");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ForumCategory");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "EmployeeRole");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "EmployeeRole");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Discount");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Discount");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ContentBlocks");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ContentBlocks");

            migrationBuilder.DropColumn(
                name: "BookingDate",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Booking");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "TripSeason",
                newName: "LastModifiedAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "TripLevelOfDifficulty",
                newName: "LastModifiedAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Trip",
                newName: "LastModifiedAt");

            migrationBuilder.RenameColumn(
                name: "UpdateDate",
                table: "Booking",
                newName: "LastModifiedAt");

            migrationBuilder.AddColumn<DateOnly>(
                name: "LastModifiedAt",
                table: "UserType",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "LastModifiedAt",
                table: "UserRole",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "LastModifiedAt",
                table: "User",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "LastModifiedAt",
                table: "PaymentMethod",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "LastModifiedAt",
                table: "Igloo",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "LastModifiedAt",
                table: "ForumPost",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "LastModifiedAt",
                table: "ForumComment",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "LastModifiedAt",
                table: "ForumCategory",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "LastModifiedAt",
                table: "EmployeeRole",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "LastModifiedAt",
                table: "Employee",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "LastModifiedAt",
                table: "Discount",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "LastModifiedAt",
                table: "Customer",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "LastModifiedAt",
                table: "ContentBlocks",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "UserType");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "UserRole");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "PaymentMethod");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "Igloo");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "ForumPost");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "ForumComment");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "ForumCategory");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "EmployeeRole");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "Discount");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "ContentBlocks");

            migrationBuilder.RenameColumn(
                name: "LastModifiedAt",
                table: "TripSeason",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "LastModifiedAt",
                table: "TripLevelOfDifficulty",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "LastModifiedAt",
                table: "Trip",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "LastModifiedAt",
                table: "Booking",
                newName: "UpdateDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "UserType",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "UserType",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "UserRole",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "UserRole",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "User",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "CreatedAt",
                table: "TripSeason",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "CreatedAt",
                table: "TripLevelOfDifficulty",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "CreatedAt",
                table: "Trip",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "PaymentMethod",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "PaymentMethod",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Igloo",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Igloo",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "PostDate",
                table: "ForumPost",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "UpdateDate",
                table: "ForumPost",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "CommentDate",
                table: "ForumComment",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ForumComment",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ForumCategory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ForumCategory",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "EmployeeRole",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "EmployeeRole",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Employee",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Employee",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Discount",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Discount",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Customer",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Customer",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ContentBlocks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ContentBlocks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "BookingDate",
                table: "Booking",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Booking",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Booking",
                type: "datetime2",
                nullable: true);
        }
    }
}
