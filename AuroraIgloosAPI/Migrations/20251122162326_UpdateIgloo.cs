using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuroraIgloosAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIgloo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Igloo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "Igloo",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Igloo");

            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "Igloo");
        }
    }
}
