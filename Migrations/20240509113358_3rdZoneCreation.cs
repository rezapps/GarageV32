using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageV32.Migrations
{
    /// <inheritdoc />
    public partial class _3rdZoneCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OccupiedSpotsList",
                table: "GarageZone",
                type: "TEXT",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OccupiedSpotsList",
                table: "GarageZone");
        }
    }
}
