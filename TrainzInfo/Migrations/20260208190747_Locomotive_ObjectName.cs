using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class Locomotive_ObjectName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DocumentToIndexid",
                table: "UserLocomotivePhotos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DocumentToIndexid",
                table: "TrainsShadule",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DocumentToIndexid",
                table: "StationsShadules",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DocumentToIndexid",
                table: "PlanningUserTrains",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DocumentToIndexid",
                table: "PlanningUserRoutes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DocumentToIndexid",
                table: "NewsComments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObjectName",
                table: "Locomotives",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DocumentToIndex",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameObject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SearchContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Locomotiveid = table.Column<int>(type: "int", nullable: true),
                    NewsInfoid = table.Column<int>(type: "int", nullable: true),
                    NewsImageid = table.Column<int>(type: "int", nullable: true),
                    Electricid = table.Column<int>(type: "int", nullable: true),
                    UserTrainzPhotoid = table.Column<int>(type: "int", nullable: true),
                    ElectrickTrainzInformationid = table.Column<int>(type: "int", nullable: true),
                    UkrainsRailwaysid = table.Column<int>(type: "int", nullable: true),
                    Depotsid = table.Column<int>(type: "int", nullable: true),
                    Citiesid = table.Column<int>(type: "int", nullable: true),
                    Oblastsid = table.Column<int>(type: "int", nullable: true),
                    Stationsid = table.Column<int>(type: "int", nullable: true),
                    TypeOfPassTrainsid = table.Column<int>(type: "int", nullable: true),
                    Trainsid = table.Column<int>(type: "int", nullable: true),
                    Locomotive_Seriesid = table.Column<int>(type: "int", nullable: true),
                    LocomotiveBaseInfosid = table.Column<int>(type: "int", nullable: true),
                    StationInfosid = table.Column<int>(type: "int", nullable: true),
                    Plantsid = table.Column<int>(type: "int", nullable: true),
                    SuburbanTrainsInfosid = table.Column<int>(type: "int", nullable: true),
                    IpAdressesid = table.Column<int>(type: "int", nullable: true),
                    RailwayUsersPhotosid = table.Column<int>(type: "int", nullable: true),
                    MainImagesid = table.Column<int>(type: "int", nullable: true),
                    Metrosid = table.Column<int>(type: "int", nullable: true),
                    MetroStationsid = table.Column<int>(type: "int", nullable: true),
                    MetroLinesid = table.Column<int>(type: "int", nullable: true),
                    DieselTrainsId = table.Column<int>(type: "int", nullable: true),
                    StationImagesid = table.Column<int>(type: "int", nullable: true),
                    MailSettingsid = table.Column<int>(type: "int", nullable: true),
                    SendEmailsId = table.Column<int>(type: "int", nullable: true),
                    PlanningUserRouteSavesID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentToIndex", x => x.id);
                    table.ForeignKey(
                        name: "FK_DocumentToIndex_Cities_Citiesid",
                        column: x => x.Citiesid,
                        principalTable: "Cities",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DocumentToIndex_Depots_Depotsid",
                        column: x => x.Depotsid,
                        principalTable: "Depots",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DocumentToIndex_DieselTrains_DieselTrainsId",
                        column: x => x.DieselTrainsId,
                        principalTable: "DieselTrains",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DocumentToIndex_ElectrickTrainzInformation_ElectrickTrainzInformationid",
                        column: x => x.ElectrickTrainzInformationid,
                        principalTable: "ElectrickTrainzInformation",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DocumentToIndex_Electrics_Electricid",
                        column: x => x.Electricid,
                        principalTable: "Electrics",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DocumentToIndex_IpAdresses_IpAdressesid",
                        column: x => x.IpAdressesid,
                        principalTable: "IpAdresses",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DocumentToIndex_LocomotiveBaseInfos_LocomotiveBaseInfosid",
                        column: x => x.LocomotiveBaseInfosid,
                        principalTable: "LocomotiveBaseInfos",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DocumentToIndex_Locomotive_Series_Locomotive_Seriesid",
                        column: x => x.Locomotive_Seriesid,
                        principalTable: "Locomotive_Series",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DocumentToIndex_Locomotives_Locomotiveid",
                        column: x => x.Locomotiveid,
                        principalTable: "Locomotives",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DocumentToIndex_MailSettings_MailSettingsid",
                        column: x => x.MailSettingsid,
                        principalTable: "MailSettings",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DocumentToIndex_MainImages_MainImagesid",
                        column: x => x.MainImagesid,
                        principalTable: "MainImages",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DocumentToIndex_MetroLines_MetroLinesid",
                        column: x => x.MetroLinesid,
                        principalTable: "MetroLines",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DocumentToIndex_MetroStations_MetroStationsid",
                        column: x => x.MetroStationsid,
                        principalTable: "MetroStations",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DocumentToIndex_Metros_Metrosid",
                        column: x => x.Metrosid,
                        principalTable: "Metros",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DocumentToIndex_NewsImages_NewsImageid",
                        column: x => x.NewsImageid,
                        principalTable: "NewsImages",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DocumentToIndex_NewsInfos_NewsInfoid",
                        column: x => x.NewsInfoid,
                        principalTable: "NewsInfos",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DocumentToIndex_Oblasts_Oblastsid",
                        column: x => x.Oblastsid,
                        principalTable: "Oblasts",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DocumentToIndex_PlanningUserRouteSaves_PlanningUserRouteSavesID",
                        column: x => x.PlanningUserRouteSavesID,
                        principalTable: "PlanningUserRouteSaves",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_DocumentToIndex_Plants_Plantsid",
                        column: x => x.Plantsid,
                        principalTable: "Plants",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DocumentToIndex_RailwayUsersPhotos_RailwayUsersPhotosid",
                        column: x => x.RailwayUsersPhotosid,
                        principalTable: "RailwayUsersPhotos",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DocumentToIndex_SendEmails_SendEmailsId",
                        column: x => x.SendEmailsId,
                        principalTable: "SendEmails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DocumentToIndex_StationImages_StationImagesid",
                        column: x => x.StationImagesid,
                        principalTable: "StationImages",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DocumentToIndex_StationInfos_StationInfosid",
                        column: x => x.StationInfosid,
                        principalTable: "StationInfos",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DocumentToIndex_Stations_Stationsid",
                        column: x => x.Stationsid,
                        principalTable: "Stations",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DocumentToIndex_SuburbanTrainsInfos_SuburbanTrainsInfosid",
                        column: x => x.SuburbanTrainsInfosid,
                        principalTable: "SuburbanTrainsInfos",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DocumentToIndex_Trains_Trainsid",
                        column: x => x.Trainsid,
                        principalTable: "Trains",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DocumentToIndex_TypeOfPassTrains_TypeOfPassTrainsid",
                        column: x => x.TypeOfPassTrainsid,
                        principalTable: "TypeOfPassTrains",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DocumentToIndex_UkrainsRailways_UkrainsRailwaysid",
                        column: x => x.UkrainsRailwaysid,
                        principalTable: "UkrainsRailways",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DocumentToIndex_UserTrainzPhotos_UserTrainzPhotoid",
                        column: x => x.UserTrainzPhotoid,
                        principalTable: "UserTrainzPhotos",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserLocomotivePhotos_DocumentToIndexid",
                table: "UserLocomotivePhotos",
                column: "DocumentToIndexid");

            migrationBuilder.CreateIndex(
                name: "IX_TrainsShadule_DocumentToIndexid",
                table: "TrainsShadule",
                column: "DocumentToIndexid");

            migrationBuilder.CreateIndex(
                name: "IX_StationsShadules_DocumentToIndexid",
                table: "StationsShadules",
                column: "DocumentToIndexid");

            migrationBuilder.CreateIndex(
                name: "IX_PlanningUserTrains_DocumentToIndexid",
                table: "PlanningUserTrains",
                column: "DocumentToIndexid");

            migrationBuilder.CreateIndex(
                name: "IX_PlanningUserRoutes_DocumentToIndexid",
                table: "PlanningUserRoutes",
                column: "DocumentToIndexid");

            migrationBuilder.CreateIndex(
                name: "IX_NewsComments_DocumentToIndexid",
                table: "NewsComments",
                column: "DocumentToIndexid");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentToIndex_Citiesid",
                table: "DocumentToIndex",
                column: "Citiesid");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentToIndex_Depotsid",
                table: "DocumentToIndex",
                column: "Depotsid");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentToIndex_DieselTrainsId",
                table: "DocumentToIndex",
                column: "DieselTrainsId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentToIndex_Electricid",
                table: "DocumentToIndex",
                column: "Electricid");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentToIndex_ElectrickTrainzInformationid",
                table: "DocumentToIndex",
                column: "ElectrickTrainzInformationid");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentToIndex_IpAdressesid",
                table: "DocumentToIndex",
                column: "IpAdressesid");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentToIndex_Locomotive_Seriesid",
                table: "DocumentToIndex",
                column: "Locomotive_Seriesid");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentToIndex_LocomotiveBaseInfosid",
                table: "DocumentToIndex",
                column: "LocomotiveBaseInfosid");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentToIndex_Locomotiveid",
                table: "DocumentToIndex",
                column: "Locomotiveid");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentToIndex_MailSettingsid",
                table: "DocumentToIndex",
                column: "MailSettingsid");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentToIndex_MainImagesid",
                table: "DocumentToIndex",
                column: "MainImagesid");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentToIndex_MetroLinesid",
                table: "DocumentToIndex",
                column: "MetroLinesid");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentToIndex_Metrosid",
                table: "DocumentToIndex",
                column: "Metrosid");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentToIndex_MetroStationsid",
                table: "DocumentToIndex",
                column: "MetroStationsid");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentToIndex_NewsImageid",
                table: "DocumentToIndex",
                column: "NewsImageid");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentToIndex_NewsInfoid",
                table: "DocumentToIndex",
                column: "NewsInfoid");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentToIndex_Oblastsid",
                table: "DocumentToIndex",
                column: "Oblastsid");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentToIndex_PlanningUserRouteSavesID",
                table: "DocumentToIndex",
                column: "PlanningUserRouteSavesID");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentToIndex_Plantsid",
                table: "DocumentToIndex",
                column: "Plantsid");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentToIndex_RailwayUsersPhotosid",
                table: "DocumentToIndex",
                column: "RailwayUsersPhotosid");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentToIndex_SendEmailsId",
                table: "DocumentToIndex",
                column: "SendEmailsId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentToIndex_StationImagesid",
                table: "DocumentToIndex",
                column: "StationImagesid");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentToIndex_StationInfosid",
                table: "DocumentToIndex",
                column: "StationInfosid");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentToIndex_Stationsid",
                table: "DocumentToIndex",
                column: "Stationsid");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentToIndex_SuburbanTrainsInfosid",
                table: "DocumentToIndex",
                column: "SuburbanTrainsInfosid");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentToIndex_Trainsid",
                table: "DocumentToIndex",
                column: "Trainsid");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentToIndex_TypeOfPassTrainsid",
                table: "DocumentToIndex",
                column: "TypeOfPassTrainsid");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentToIndex_UkrainsRailwaysid",
                table: "DocumentToIndex",
                column: "UkrainsRailwaysid");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentToIndex_UserTrainzPhotoid",
                table: "DocumentToIndex",
                column: "UserTrainzPhotoid");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsComments_DocumentToIndex_DocumentToIndexid",
                table: "NewsComments",
                column: "DocumentToIndexid",
                principalTable: "DocumentToIndex",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanningUserRoutes_DocumentToIndex_DocumentToIndexid",
                table: "PlanningUserRoutes",
                column: "DocumentToIndexid",
                principalTable: "DocumentToIndex",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanningUserTrains_DocumentToIndex_DocumentToIndexid",
                table: "PlanningUserTrains",
                column: "DocumentToIndexid",
                principalTable: "DocumentToIndex",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_StationsShadules_DocumentToIndex_DocumentToIndexid",
                table: "StationsShadules",
                column: "DocumentToIndexid",
                principalTable: "DocumentToIndex",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainsShadule_DocumentToIndex_DocumentToIndexid",
                table: "TrainsShadule",
                column: "DocumentToIndexid",
                principalTable: "DocumentToIndex",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLocomotivePhotos_DocumentToIndex_DocumentToIndexid",
                table: "UserLocomotivePhotos",
                column: "DocumentToIndexid",
                principalTable: "DocumentToIndex",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsComments_DocumentToIndex_DocumentToIndexid",
                table: "NewsComments");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanningUserRoutes_DocumentToIndex_DocumentToIndexid",
                table: "PlanningUserRoutes");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanningUserTrains_DocumentToIndex_DocumentToIndexid",
                table: "PlanningUserTrains");

            migrationBuilder.DropForeignKey(
                name: "FK_StationsShadules_DocumentToIndex_DocumentToIndexid",
                table: "StationsShadules");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainsShadule_DocumentToIndex_DocumentToIndexid",
                table: "TrainsShadule");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLocomotivePhotos_DocumentToIndex_DocumentToIndexid",
                table: "UserLocomotivePhotos");

            migrationBuilder.DropTable(
                name: "DocumentToIndex");

            migrationBuilder.DropIndex(
                name: "IX_UserLocomotivePhotos_DocumentToIndexid",
                table: "UserLocomotivePhotos");

            migrationBuilder.DropIndex(
                name: "IX_TrainsShadule_DocumentToIndexid",
                table: "TrainsShadule");

            migrationBuilder.DropIndex(
                name: "IX_StationsShadules_DocumentToIndexid",
                table: "StationsShadules");

            migrationBuilder.DropIndex(
                name: "IX_PlanningUserTrains_DocumentToIndexid",
                table: "PlanningUserTrains");

            migrationBuilder.DropIndex(
                name: "IX_PlanningUserRoutes_DocumentToIndexid",
                table: "PlanningUserRoutes");

            migrationBuilder.DropIndex(
                name: "IX_NewsComments_DocumentToIndexid",
                table: "NewsComments");

            migrationBuilder.DropColumn(
                name: "DocumentToIndexid",
                table: "UserLocomotivePhotos");

            migrationBuilder.DropColumn(
                name: "DocumentToIndexid",
                table: "TrainsShadule");

            migrationBuilder.DropColumn(
                name: "DocumentToIndexid",
                table: "StationsShadules");

            migrationBuilder.DropColumn(
                name: "DocumentToIndexid",
                table: "PlanningUserTrains");

            migrationBuilder.DropColumn(
                name: "DocumentToIndexid",
                table: "PlanningUserRoutes");

            migrationBuilder.DropColumn(
                name: "DocumentToIndexid",
                table: "NewsComments");

            migrationBuilder.DropColumn(
                name: "ObjectName",
                table: "Locomotives");
        }
    }
}
