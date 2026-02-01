using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class PlanningUserRoute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "NewsInfos",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PlanningUserRoutes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TrainsShaduleID = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanningUserRoutes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PlanningUserRoutes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlanningUserRoutes_TrainsShadule_TrainsShaduleID",
                        column: x => x.TrainsShaduleID,
                        principalTable: "TrainsShadule",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanningUserTrains",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainID = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanningUserTrains", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PlanningUserTrains_Trains_TrainID",
                        column: x => x.TrainID,
                        principalTable: "Trains",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanningUserRouteSaves",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlanningUserRouteID = table.Column<int>(type: "int", nullable: false),
                    PlanningUserTrainsID = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanningUserRouteSaves", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PlanningUserRouteSaves_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlanningUserRouteSaves_PlanningUserRoutes_PlanningUserRouteID",
                        column: x => x.PlanningUserRouteID,
                        principalTable: "PlanningUserRoutes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanningUserRouteSaves_PlanningUserTrains_PlanningUserTrainsID",
                        column: x => x.PlanningUserTrainsID,
                        principalTable: "PlanningUserTrains",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NewsInfos_UserId",
                table: "NewsInfos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanningUserRoutes_TrainsShaduleID",
                table: "PlanningUserRoutes",
                column: "TrainsShaduleID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanningUserRoutes_UserId",
                table: "PlanningUserRoutes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanningUserRouteSaves_PlanningUserRouteID",
                table: "PlanningUserRouteSaves",
                column: "PlanningUserRouteID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanningUserRouteSaves_PlanningUserTrainsID",
                table: "PlanningUserRouteSaves",
                column: "PlanningUserTrainsID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanningUserRouteSaves_UserId",
                table: "PlanningUserRouteSaves",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanningUserTrains_TrainID",
                table: "PlanningUserTrains",
                column: "TrainID");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsInfos_AspNetUsers_UserId",
                table: "NewsInfos",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsInfos_AspNetUsers_UserId",
                table: "NewsInfos");

            migrationBuilder.DropTable(
                name: "PlanningUserRouteSaves");

            migrationBuilder.DropTable(
                name: "PlanningUserRoutes");

            migrationBuilder.DropTable(
                name: "PlanningUserTrains");

            migrationBuilder.DropIndex(
                name: "IX_NewsInfos_UserId",
                table: "NewsInfos");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "NewsInfos");
        }
    }
}
