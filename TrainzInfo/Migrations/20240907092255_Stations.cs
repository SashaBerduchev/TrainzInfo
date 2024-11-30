using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class Stations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Citysid",
                table: "Stations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Oblastsid",
                table: "Stations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UkrainsRailwaysid",
                table: "Stations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stations_Citysid",
                table: "Stations",
                column: "Citysid");

            migrationBuilder.CreateIndex(
                name: "IX_Stations_Oblastsid",
                table: "Stations",
                column: "Oblastsid");

            migrationBuilder.CreateIndex(
                name: "IX_Stations_UkrainsRailwaysid",
                table: "Stations",
                column: "UkrainsRailwaysid");

            migrationBuilder.AddForeignKey(
                name: "FK_Stations_Cities_Citysid",
                table: "Stations",
                column: "Citysid",
                principalTable: "Cities",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Stations_Oblasts_Oblastsid",
                table: "Stations",
                column: "Oblastsid",
                principalTable: "Oblasts",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Stations_UkrainsRailways_UkrainsRailwaysid",
                table: "Stations",
                column: "UkrainsRailwaysid",
                principalTable: "UkrainsRailways",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stations_Cities_Citysid",
                table: "Stations");

            migrationBuilder.DropForeignKey(
                name: "FK_Stations_Oblasts_Oblastsid",
                table: "Stations");

            migrationBuilder.DropForeignKey(
                name: "FK_Stations_UkrainsRailways_UkrainsRailwaysid",
                table: "Stations");

            migrationBuilder.DropIndex(
                name: "IX_Stations_Citysid",
                table: "Stations");

            migrationBuilder.DropIndex(
                name: "IX_Stations_Oblastsid",
                table: "Stations");

            migrationBuilder.DropIndex(
                name: "IX_Stations_UkrainsRailwaysid",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "Citysid",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "Oblastsid",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "UkrainsRailwaysid",
                table: "Stations");
        }
    }
}
