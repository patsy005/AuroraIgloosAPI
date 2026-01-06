using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuroraIgloosAPI.Migrations
{
    /// <inheritdoc />
    public partial class EmpployeeTrip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trip_Employee_GuideId",
                table: "Trip");

            migrationBuilder.AddForeignKey(
                name: "FK_Trip_Employee_GuideId",
                table: "Trip",
                column: "GuideId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trip_Employee_GuideId",
                table: "Trip");

            migrationBuilder.AddForeignKey(
                name: "FK_Trip_Employee_GuideId",
                table: "Trip",
                column: "GuideId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
