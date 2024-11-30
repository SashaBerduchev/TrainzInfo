using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class StationInfoFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Stationsid",
                table: "stationInfos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_stationInfos_Stationsid",
                table: "stationInfos",
                column: "Stationsid");

            migrationBuilder.AddForeignKey(
                name: "FK_stationInfos_Stations_Stationsid",
                table: "stationInfos",
                column: "Stationsid",
                principalTable: "Stations",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_stationInfos_Stations_Stationsid",
                table: "stationInfos");

            migrationBuilder.DropIndex(
                name: "IX_stationInfos_Stationsid",
                table: "stationInfos");

            migrationBuilder.DropColumn(
                name: "Stationsid",
                table: "stationInfos");
        }
    }
}
