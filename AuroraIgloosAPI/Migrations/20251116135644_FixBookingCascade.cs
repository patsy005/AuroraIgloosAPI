using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuroraIgloosAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixBookingCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Customer_IdCustomer",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Employee_CreatedById",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Igloo_IdIgloo",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_PaymentMethod_PaymentMethodId",
                table: "Booking");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Customer_IdCustomer",
                table: "Booking",
                column: "IdCustomer",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Employee_CreatedById",
                table: "Booking",
                column: "CreatedById",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Igloo_IdIgloo",
                table: "Booking",
                column: "IdIgloo",
                principalTable: "Igloo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_PaymentMethod_PaymentMethodId",
                table: "Booking",
                column: "PaymentMethodId",
                principalTable: "PaymentMethod",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Customer_IdCustomer",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Employee_CreatedById",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Igloo_IdIgloo",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_PaymentMethod_PaymentMethodId",
                table: "Booking");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Customer_IdCustomer",
                table: "Booking",
                column: "IdCustomer",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Employee_CreatedById",
                table: "Booking",
                column: "CreatedById",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Igloo_IdIgloo",
                table: "Booking",
                column: "IdIgloo",
                principalTable: "Igloo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_PaymentMethod_PaymentMethodId",
                table: "Booking",
                column: "PaymentMethodId",
                principalTable: "PaymentMethod",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
