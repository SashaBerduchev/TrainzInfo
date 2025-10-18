using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class Stations_Add_StationImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StationImages_Stations_Stationsid",
                table: "StationImages");

            migrationBuilder.DropIndex(
                name: "IX_StationImages_Stationsid",
                table: "StationImages");

            migrationBuilder.DropColumn(
                name: "Stationsid",
                table: "StationImages");

            migrationBuilder.AddColumn<int>(
                name: "StationImagesid",
                table: "Stations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stations_StationImagesid",
                table: "Stations",
                column: "StationImagesid");

            migrationBuilder.AddForeignKey(
                name: "FK_Stations_StationImages_StationImagesid",
                table: "Stations",
                column: "StationImagesid",
                principalTable: "StationImages",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stations_StationImages_StationImagesid",
                table: "Stations");

            migrationBuilder.DropIndex(
                name: "IX_Stations_StationImagesid",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "StationImagesid",
                table: "Stations");

            migrationBuilder.AddColumn<int>(
                name: "Stationsid",
                table: "StationImages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StationImages_Stationsid",
                table: "StationImages",
                column: "Stationsid");

            migrationBuilder.AddForeignKey(
                name: "FK_StationImages_Stations_Stationsid",
                table: "StationImages",
                column: "Stationsid",
                principalTable: "Stations",
                principalColumn: "id");
        }
    }
}
