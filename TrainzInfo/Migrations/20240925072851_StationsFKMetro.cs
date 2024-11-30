using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class StationsFKMetro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Metro",
                table: "MetroLines");

            migrationBuilder.AddColumn<int>(
                name: "Metroid",
                table: "Stations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MetroLinesid",
                table: "MetroStations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Metroid",
                table: "MetroLines",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stations_Metroid",
                table: "Stations",
                column: "Metroid");

            migrationBuilder.CreateIndex(
                name: "IX_MetroStations_MetroLinesid",
                table: "MetroStations",
                column: "MetroLinesid");

            migrationBuilder.CreateIndex(
                name: "IX_MetroLines_Metroid",
                table: "MetroLines",
                column: "Metroid");

            migrationBuilder.AddForeignKey(
                name: "FK_MetroLines_Metros_Metroid",
                table: "MetroLines",
                column: "Metroid",
                principalTable: "Metros",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_MetroStations_MetroLines_MetroLinesid",
                table: "MetroStations",
                column: "MetroLinesid",
                principalTable: "MetroLines",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Stations_Metros_Metroid",
                table: "Stations",
                column: "Metroid",
                principalTable: "Metros",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MetroLines_Metros_Metroid",
                table: "MetroLines");

            migrationBuilder.DropForeignKey(
                name: "FK_MetroStations_MetroLines_MetroLinesid",
                table: "MetroStations");

            migrationBuilder.DropForeignKey(
                name: "FK_Stations_Metros_Metroid",
                table: "Stations");

            migrationBuilder.DropIndex(
                name: "IX_Stations_Metroid",
                table: "Stations");

            migrationBuilder.DropIndex(
                name: "IX_MetroStations_MetroLinesid",
                table: "MetroStations");

            migrationBuilder.DropIndex(
                name: "IX_MetroLines_Metroid",
                table: "MetroLines");

            migrationBuilder.DropColumn(
                name: "Metroid",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "MetroLinesid",
                table: "MetroStations");

            migrationBuilder.DropColumn(
                name: "Metroid",
                table: "MetroLines");

            migrationBuilder.AddColumn<string>(
                name: "Metro",
                table: "MetroLines",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
