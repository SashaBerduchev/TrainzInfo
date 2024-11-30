using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class StationsShaduleTrainSheduller : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StationsShaduleTrainsShadule");

            //migrationBuilder.DropTable(
            //    name: "StationsTrainsShadule");

            migrationBuilder.DropColumn(
                name: "ImgTrain",
                table: "StationsShadules");

            migrationBuilder.AddColumn<int>(
                name: "Stationsid",
                table: "TrainsShadule",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainsShadule_Stationsid",
                table: "TrainsShadule",
                column: "Stationsid");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainsShadule_Stations_Stationsid",
                table: "TrainsShadule",
                column: "Stationsid",
                principalTable: "Stations",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainsShadule_Stations_Stationsid",
                table: "TrainsShadule");

            migrationBuilder.DropIndex(
                name: "IX_TrainsShadule_Stationsid",
                table: "TrainsShadule");

            migrationBuilder.DropColumn(
                name: "Stationsid",
                table: "TrainsShadule");

            migrationBuilder.AddColumn<string>(
                name: "ImgTrain",
                table: "StationsShadules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StationsShaduleTrainsShadule",
                columns: table => new
                {
                    TrainsShadulesid = table.Column<int>(type: "int", nullable: false),
                    stationsShadulesid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StationsShaduleTrainsShadule", x => new { x.TrainsShadulesid, x.stationsShadulesid });
                    table.ForeignKey(
                        name: "FK_StationsShaduleTrainsShadule_StationsShadules_stationsShadulesid",
                        column: x => x.stationsShadulesid,
                        principalTable: "StationsShadules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StationsShaduleTrainsShadule_TrainsShadule_TrainsShadulesid",
                        column: x => x.TrainsShadulesid,
                        principalTable: "TrainsShadule",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StationsTrainsShadule",
                columns: table => new
                {
                    Stationsid = table.Column<int>(type: "int", nullable: false),
                    TrainsShadulesid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StationsTrainsShadule", x => new { x.Stationsid, x.TrainsShadulesid });
                    table.ForeignKey(
                        name: "FK_StationsTrainsShadule_Stations_Stationsid",
                        column: x => x.Stationsid,
                        principalTable: "Stations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StationsTrainsShadule_TrainsShadule_TrainsShadulesid",
                        column: x => x.TrainsShadulesid,
                        principalTable: "TrainsShadule",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StationsShaduleTrainsShadule_stationsShadulesid",
                table: "StationsShaduleTrainsShadule",
                column: "stationsShadulesid");

            migrationBuilder.CreateIndex(
                name: "IX_StationsTrainsShadule_TrainsShadulesid",
                table: "StationsTrainsShadule",
                column: "TrainsShadulesid");
        }
    }
}
