using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class StationsFKStationsInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_stationInfos_Stations_Stationsid",
                table: "stationInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_stationInfos_stationInfos_StationInfoid",
                table: "stationInfos");

            migrationBuilder.DropIndex(
                name: "IX_stationInfos_StationInfoid",
                table: "stationInfos");

            migrationBuilder.DropIndex(
                name: "IX_stationInfos_Stationsid",
                table: "stationInfos");

            migrationBuilder.DropColumn(
                name: "StationInfoid",
                table: "stationInfos");

            migrationBuilder.DropColumn(
                name: "Stationsid",
                table: "stationInfos");

            migrationBuilder.AddColumn<int>(
                name: "StationInfoid",
                table: "Stations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stations_StationInfoid",
                table: "Stations",
                column: "StationInfoid");

            migrationBuilder.AddForeignKey(
                name: "FK_Stations_stationInfos_StationInfoid",
                table: "Stations",
                column: "StationInfoid",
                principalTable: "stationInfos",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stations_stationInfos_StationInfoid",
                table: "Stations");

            migrationBuilder.DropIndex(
                name: "IX_Stations_StationInfoid",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "StationInfoid",
                table: "Stations");

            migrationBuilder.AddColumn<int>(
                name: "StationInfoid",
                table: "stationInfos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Stationsid",
                table: "stationInfos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_stationInfos_StationInfoid",
                table: "stationInfos",
                column: "StationInfoid");

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

            migrationBuilder.AddForeignKey(
                name: "FK_stationInfos_stationInfos_StationInfoid",
                table: "stationInfos",
                column: "StationInfoid",
                principalTable: "stationInfos",
                principalColumn: "id");
        }
    }
}
