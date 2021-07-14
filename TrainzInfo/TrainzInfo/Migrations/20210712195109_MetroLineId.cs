using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainzInfo.Migrations
{
    public partial class MetroLineId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MetroLine",
                table: "MetroStations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MetroLineId",
                table: "MetroStations",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MetroLine",
                table: "MetroStations");

            migrationBuilder.DropColumn(
                name: "MetroLineId",
                table: "MetroStations");
        }
    }
}
