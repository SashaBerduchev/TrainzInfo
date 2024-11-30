using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class UserTrainzPhotoFKStations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocmotiveName",
                table: "UserTrainzPhotos");

            migrationBuilder.DropColumn(
                name: "Marshrute",
                table: "UserTrainzPhotos");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserTrainzPhotos");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "UserTrainzPhotos");

            migrationBuilder.AddColumn<int>(
                name: "Stationsid",
                table: "UserTrainzPhotos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UseridId",
                table: "UserTrainzPhotos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StationInfoid",
                table: "stationInfos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserTrainzPhotos_Stationsid",
                table: "UserTrainzPhotos",
                column: "Stationsid");

            migrationBuilder.CreateIndex(
                name: "IX_UserTrainzPhotos_UseridId",
                table: "UserTrainzPhotos",
                column: "UseridId");

            migrationBuilder.CreateIndex(
                name: "IX_stationInfos_StationInfoid",
                table: "stationInfos",
                column: "StationInfoid");

            migrationBuilder.AddForeignKey(
                name: "FK_stationInfos_stationInfos_StationInfoid",
                table: "stationInfos",
                column: "StationInfoid",
                principalTable: "stationInfos",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTrainzPhotos_Stations_Stationsid",
                table: "UserTrainzPhotos",
                column: "Stationsid",
                principalTable: "Stations",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTrainzPhotos_User_UseridId",
                table: "UserTrainzPhotos",
                column: "UseridId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_stationInfos_stationInfos_StationInfoid",
                table: "stationInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTrainzPhotos_Stations_Stationsid",
                table: "UserTrainzPhotos");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTrainzPhotos_User_UseridId",
                table: "UserTrainzPhotos");

            migrationBuilder.DropIndex(
                name: "IX_UserTrainzPhotos_Stationsid",
                table: "UserTrainzPhotos");

            migrationBuilder.DropIndex(
                name: "IX_UserTrainzPhotos_UseridId",
                table: "UserTrainzPhotos");

            migrationBuilder.DropIndex(
                name: "IX_stationInfos_StationInfoid",
                table: "stationInfos");

            migrationBuilder.DropColumn(
                name: "Stationsid",
                table: "UserTrainzPhotos");

            migrationBuilder.DropColumn(
                name: "UseridId",
                table: "UserTrainzPhotos");

            migrationBuilder.DropColumn(
                name: "StationInfoid",
                table: "stationInfos");

            migrationBuilder.AddColumn<string>(
                name: "LocmotiveName",
                table: "UserTrainzPhotos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Marshrute",
                table: "UserTrainzPhotos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "UserTrainzPhotos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "UserTrainzPhotos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
