using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class StationsShaduleFK_UkraineRailway : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UkrainsRailwaysid",
                table: "StationsShadules",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StationsShadules_UkrainsRailwaysid",
                table: "StationsShadules",
                column: "UkrainsRailwaysid");

            migrationBuilder.AddForeignKey(
                name: "FK_StationsShadules_UkrainsRailways_UkrainsRailwaysid",
                table: "StationsShadules",
                column: "UkrainsRailwaysid",
                principalTable: "UkrainsRailways",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StationsShadules_UkrainsRailways_UkrainsRailwaysid",
                table: "StationsShadules");

            migrationBuilder.DropIndex(
                name: "IX_StationsShadules_UkrainsRailwaysid",
                table: "StationsShadules");

            migrationBuilder.DropColumn(
                name: "UkrainsRailwaysid",
                table: "StationsShadules");
        }
    }
}
