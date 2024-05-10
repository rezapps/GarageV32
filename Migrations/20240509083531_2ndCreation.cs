using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageV32.Migrations
{
    /// <inheritdoc />
    public partial class _2ndCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SpotNumber2",
                table: "ParkedVehicle",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpotNumber3",
                table: "ParkedVehicle",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpotNumber2",
                table: "ParkedVehicle");

            migrationBuilder.DropColumn(
                name: "SpotNumber3",
                table: "ParkedVehicle");
        }
    }
}
