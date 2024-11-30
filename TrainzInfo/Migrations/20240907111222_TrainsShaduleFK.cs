using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class TrainsShaduleFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "IX_StationsTrainsShadule_TrainsShadulesid",
                table: "StationsTrainsShadule",
                column: "TrainsShadulesid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StationsTrainsShadule");
        }
    }
}
