using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class TrainzStations_Distancef : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Distance",
                table: "TrainzStations");

            migrationBuilder.AddColumn<string>(
                name: "Distance",
                table: "TrainsShadule",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Distance",
                table: "TrainsShadule");

            migrationBuilder.AddColumn<string>(
                name: "Distance",
                table: "TrainzStations",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
