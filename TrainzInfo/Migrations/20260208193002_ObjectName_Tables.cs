using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class ObjectName_Tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ObjectName",
                table: "UserTrainzPhotos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObjectName",
                table: "UserLocomotivePhotos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObjectName",
                table: "UkrainsRailways",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObjectName",
                table: "TypeOfPassTrains",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObjectName",
                table: "TrainsShadule",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObjectName",
                table: "Trains",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObjectName",
                table: "SuburbanTrainsInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObjectName",
                table: "StationsShadules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObjectName",
                table: "Stations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObjectName",
                table: "StationInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObjectName",
                table: "StationImages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObjectName",
                table: "SendEmails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObjectName",
                table: "RailwayUsersPhotos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObjectName",
                table: "Plants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObjectName",
                table: "PlanningUserTrains",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObjectName",
                table: "PlanningUserRouteSaves",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObjectName",
                table: "PlanningUserRoutes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObjectName",
                table: "Oblasts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObjectName",
                table: "NewsInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObjectName",
                table: "NewsImages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObjectName",
                table: "NewsComments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObjectName",
                table: "MetroStations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObjectName",
                table: "Metros",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObjectName",
                table: "MetroLines",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObjectName",
                table: "MainImages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObjectName",
                table: "MailSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObjectName",
                table: "LocomotiveBaseInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObjectName",
                table: "Locomotive_Series",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObjectName",
                table: "Electrics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObjectName",
                table: "ElectrickTrainzInformation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObjectName",
                table: "DieselTrains",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObjectName",
                table: "Depots",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObjectName",
                table: "Cities",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ObjectName",
                table: "UserTrainzPhotos");

            migrationBuilder.DropColumn(
                name: "ObjectName",
                table: "UserLocomotivePhotos");

            migrationBuilder.DropColumn(
                name: "ObjectName",
                table: "UkrainsRailways");

            migrationBuilder.DropColumn(
                name: "ObjectName",
                table: "TypeOfPassTrains");

            migrationBuilder.DropColumn(
                name: "ObjectName",
                table: "TrainsShadule");

            migrationBuilder.DropColumn(
                name: "ObjectName",
                table: "Trains");

            migrationBuilder.DropColumn(
                name: "ObjectName",
                table: "SuburbanTrainsInfos");

            migrationBuilder.DropColumn(
                name: "ObjectName",
                table: "StationsShadules");

            migrationBuilder.DropColumn(
                name: "ObjectName",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "ObjectName",
                table: "StationInfos");

            migrationBuilder.DropColumn(
                name: "ObjectName",
                table: "StationImages");

            migrationBuilder.DropColumn(
                name: "ObjectName",
                table: "SendEmails");

            migrationBuilder.DropColumn(
                name: "ObjectName",
                table: "RailwayUsersPhotos");

            migrationBuilder.DropColumn(
                name: "ObjectName",
                table: "Plants");

            migrationBuilder.DropColumn(
                name: "ObjectName",
                table: "PlanningUserTrains");

            migrationBuilder.DropColumn(
                name: "ObjectName",
                table: "PlanningUserRouteSaves");

            migrationBuilder.DropColumn(
                name: "ObjectName",
                table: "PlanningUserRoutes");

            migrationBuilder.DropColumn(
                name: "ObjectName",
                table: "Oblasts");

            migrationBuilder.DropColumn(
                name: "ObjectName",
                table: "NewsInfos");

            migrationBuilder.DropColumn(
                name: "ObjectName",
                table: "NewsImages");

            migrationBuilder.DropColumn(
                name: "ObjectName",
                table: "NewsComments");

            migrationBuilder.DropColumn(
                name: "ObjectName",
                table: "MetroStations");

            migrationBuilder.DropColumn(
                name: "ObjectName",
                table: "Metros");

            migrationBuilder.DropColumn(
                name: "ObjectName",
                table: "MetroLines");

            migrationBuilder.DropColumn(
                name: "ObjectName",
                table: "MainImages");

            migrationBuilder.DropColumn(
                name: "ObjectName",
                table: "MailSettings");

            migrationBuilder.DropColumn(
                name: "ObjectName",
                table: "LocomotiveBaseInfos");

            migrationBuilder.DropColumn(
                name: "ObjectName",
                table: "Locomotive_Series");

            migrationBuilder.DropColumn(
                name: "ObjectName",
                table: "Electrics");

            migrationBuilder.DropColumn(
                name: "ObjectName",
                table: "ElectrickTrainzInformation");

            migrationBuilder.DropColumn(
                name: "ObjectName",
                table: "DieselTrains");

            migrationBuilder.DropColumn(
                name: "ObjectName",
                table: "Depots");

            migrationBuilder.DropColumn(
                name: "ObjectName",
                table: "Cities");
        }
    }
}
