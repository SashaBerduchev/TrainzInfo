using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class TablesUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Electrics_plants_Plantsid",
                table: "Electrics");

            migrationBuilder.DropForeignKey(
                name: "FK_Locomotives_locomotiveBaseInfos_LocomotiveBaseInfoid",
                table: "Locomotives");

            migrationBuilder.DropForeignKey(
                name: "FK_Stations_stationInfos_StationInfoid",
                table: "Stations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_stationInfos",
                table: "stationInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_plants",
                table: "plants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_locomotiveBaseInfos",
                table: "locomotiveBaseInfos");

            migrationBuilder.RenameTable(
                name: "stationInfos",
                newName: "StationInfos");

            migrationBuilder.RenameTable(
                name: "plants",
                newName: "Plants");

            migrationBuilder.RenameTable(
                name: "locomotiveBaseInfos",
                newName: "LocomotiveBaseInfos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StationInfos",
                table: "StationInfos",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Plants",
                table: "Plants",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LocomotiveBaseInfos",
                table: "LocomotiveBaseInfos",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Electrics_Plants_Plantsid",
                table: "Electrics",
                column: "Plantsid",
                principalTable: "Plants",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Locomotives_LocomotiveBaseInfos_LocomotiveBaseInfoid",
                table: "Locomotives",
                column: "LocomotiveBaseInfoid",
                principalTable: "LocomotiveBaseInfos",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Stations_StationInfos_StationInfoid",
                table: "Stations",
                column: "StationInfoid",
                principalTable: "StationInfos",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Electrics_Plants_Plantsid",
                table: "Electrics");

            migrationBuilder.DropForeignKey(
                name: "FK_Locomotives_LocomotiveBaseInfos_LocomotiveBaseInfoid",
                table: "Locomotives");

            migrationBuilder.DropForeignKey(
                name: "FK_Stations_StationInfos_StationInfoid",
                table: "Stations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StationInfos",
                table: "StationInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Plants",
                table: "Plants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LocomotiveBaseInfos",
                table: "LocomotiveBaseInfos");

            migrationBuilder.RenameTable(
                name: "StationInfos",
                newName: "stationInfos");

            migrationBuilder.RenameTable(
                name: "Plants",
                newName: "plants");

            migrationBuilder.RenameTable(
                name: "LocomotiveBaseInfos",
                newName: "locomotiveBaseInfos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_stationInfos",
                table: "stationInfos",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_plants",
                table: "plants",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_locomotiveBaseInfos",
                table: "locomotiveBaseInfos",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Electrics_plants_Plantsid",
                table: "Electrics",
                column: "Plantsid",
                principalTable: "plants",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Locomotives_locomotiveBaseInfos_LocomotiveBaseInfoid",
                table: "Locomotives",
                column: "LocomotiveBaseInfoid",
                principalTable: "locomotiveBaseInfos",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Stations_stationInfos_StationInfoid",
                table: "Stations",
                column: "StationInfoid",
                principalTable: "stationInfos",
                principalColumn: "id");
        }
    }
}
