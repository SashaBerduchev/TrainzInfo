using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class StationsShaduleFK_StationsFK_TrainsShadule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TrainInfo",
                table: "StationsShadules",
                newName: "NumberTrain");

            migrationBuilder.AddColumn<int>(
                name: "Stationsid",
                table: "StationsShadules",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Trainid",
                table: "StationsShadules",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StationsShadules_Stationsid",
                table: "StationsShadules",
                column: "Stationsid");

            migrationBuilder.CreateIndex(
                name: "IX_StationsShadules_Trainid",
                table: "StationsShadules",
                column: "Trainid");

            migrationBuilder.AddForeignKey(
                name: "FK_StationsShadules_Stations_Stationsid",
                table: "StationsShadules",
                column: "Stationsid",
                principalTable: "Stations",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_StationsShadules_Trains_Trainid",
                table: "StationsShadules",
                column: "Trainid",
                principalTable: "Trains",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StationsShadules_Stations_Stationsid",
                table: "StationsShadules");

            migrationBuilder.DropForeignKey(
                name: "FK_StationsShadules_Trains_Trainid",
                table: "StationsShadules");

            migrationBuilder.DropIndex(
                name: "IX_StationsShadules_Stationsid",
                table: "StationsShadules");

            migrationBuilder.DropIndex(
                name: "IX_StationsShadules_Trainid",
                table: "StationsShadules");

            migrationBuilder.DropColumn(
                name: "Stationsid",
                table: "StationsShadules");

            migrationBuilder.DropColumn(
                name: "Trainid",
                table: "StationsShadules");

            migrationBuilder.RenameColumn(
                name: "NumberTrain",
                table: "StationsShadules",
                newName: "TrainInfo");
        }
    }
}
