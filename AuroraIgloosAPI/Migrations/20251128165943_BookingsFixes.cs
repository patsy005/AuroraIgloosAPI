using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuroraIgloosAPI.Migrations
{
    /// <inheritdoc />
    public partial class BookingsFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Trip",
                newName: "PricePerPerson");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "CheckOut",
                table: "Booking",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "CheckIn",
                table: "Booking",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<int>(
                name: "Guests",
                table: "Booking",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateOnly>(
                name: "TripDate",
                table: "Booking",
                type: "date",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Guests",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "TripDate",
                table: "Booking");

            migrationBuilder.RenameColumn(
                name: "PricePerPerson",
                table: "Trip",
                newName: "Price");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "CheckOut",
                table: "Booking",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "CheckIn",
                table: "Booking",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);
        }
    }
}
