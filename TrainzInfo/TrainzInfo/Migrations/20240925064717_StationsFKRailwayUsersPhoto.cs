using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class StationsFKRailwayUsersPhoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Stationsid",
                table: "RailwayUsersPhotos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RailwayUsersPhotos_Stationsid",
                table: "RailwayUsersPhotos",
                column: "Stationsid");

            migrationBuilder.AddForeignKey(
                name: "FK_RailwayUsersPhotos_Stations_Stationsid",
                table: "RailwayUsersPhotos",
                column: "Stationsid",
                principalTable: "Stations",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RailwayUsersPhotos_Stations_Stationsid",
                table: "RailwayUsersPhotos");

            migrationBuilder.DropIndex(
                name: "IX_RailwayUsersPhotos_Stationsid",
                table: "RailwayUsersPhotos");

            migrationBuilder.DropColumn(
                name: "Stationsid",
                table: "RailwayUsersPhotos");
        }
    }
}
