using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuroraIgloosAPI.Migrations
{
    /// <inheritdoc />
    public partial class Bookings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Employee_CreatedById",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_CreatedById",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "CancellationReason",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "Booking");

            migrationBuilder.AlterColumn<int>(
                name: "IdIgloo",
                table: "Booking",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "TripId",
                table: "Booking",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "UpdateDate",
                table: "Booking",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.CreateIndex(
                name: "IX_Booking_TripId",
                table: "Booking",
                column: "TripId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Trip_TripId",
                table: "Booking",
                column: "TripId",
                principalTable: "Trip",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Trip_TripId",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_TripId",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "TripId",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "Booking");

            migrationBuilder.AlterColumn<int>(
                name: "IdIgloo",
                table: "Booking",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CancellationReason",
                table: "Booking",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Booking",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateOnly>(
                name: "LastModifiedDate",
                table: "Booking",
                type: "date",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Booking_CreatedById",
                table: "Booking",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Employee_CreatedById",
                table: "Booking",
                column: "CreatedById",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
