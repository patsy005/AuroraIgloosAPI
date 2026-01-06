using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuroraIgloosAPI.Migrations
{
    /// <inheritdoc />
    public partial class Discounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discount_Igloo_IdIgloo",
                table: "Discount");

            migrationBuilder.DropIndex(
                name: "IX_Discount_IdIgloo",
                table: "Discount");

            migrationBuilder.DropColumn(
                name: "IdIgloo",
                table: "Discount");

            migrationBuilder.AddColumn<int>(
                name: "IdDiscount",
                table: "Igloo",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "ValidFrom",
                table: "Discount",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "ValidTo",
                table: "Discount",
                type: "date",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Igloo_IdDiscount",
                table: "Igloo",
                column: "IdDiscount");

            migrationBuilder.AddForeignKey(
                name: "FK_Igloo_Discount_IdDiscount",
                table: "Igloo",
                column: "IdDiscount",
                principalTable: "Discount",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Igloo_Discount_IdDiscount",
                table: "Igloo");

            migrationBuilder.DropIndex(
                name: "IX_Igloo_IdDiscount",
                table: "Igloo");

            migrationBuilder.DropColumn(
                name: "IdDiscount",
                table: "Igloo");

            migrationBuilder.DropColumn(
                name: "ValidFrom",
                table: "Discount");

            migrationBuilder.DropColumn(
                name: "ValidTo",
                table: "Discount");

            migrationBuilder.AddColumn<int>(
                name: "IdIgloo",
                table: "Discount",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Discount_IdIgloo",
                table: "Discount",
                column: "IdIgloo");

            migrationBuilder.AddForeignKey(
                name: "FK_Discount_Igloo_IdIgloo",
                table: "Discount",
                column: "IdIgloo",
                principalTable: "Igloo",
                principalColumn: "Id");
        }
    }
}
