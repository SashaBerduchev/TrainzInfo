using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class StationsShadule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_StationsShaduleTrainsShadule_stationsShadulesid",
                table: "StationsShaduleTrainsShadule",
                column: "stationsShadulesid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StationsShaduleTrainsShadule");
        }
    }
}
