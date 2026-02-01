using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class PlanningUserRouteSave : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanningUserRoutes_TrainsShadule_TrainsShaduleID",
                table: "PlanningUserRoutes");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanningUserRouteSaves_PlanningUserRoutes_PlanningUserRouteID",
                table: "PlanningUserRouteSaves");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanningUserRouteSaves_PlanningUserTrains_PlanningUserTrainsID",
                table: "PlanningUserRouteSaves");

            migrationBuilder.DropIndex(
                name: "IX_PlanningUserRouteSaves_PlanningUserRouteID",
                table: "PlanningUserRouteSaves");

            migrationBuilder.DropIndex(
                name: "IX_PlanningUserRouteSaves_PlanningUserTrainsID",
                table: "PlanningUserRouteSaves");

            migrationBuilder.DropIndex(
                name: "IX_PlanningUserRoutes_TrainsShaduleID",
                table: "PlanningUserRoutes");

            migrationBuilder.AddColumn<int>(
                name: "PlanningUserRouteID",
                table: "TrainsShadule",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlanningUserRouteSaveID",
                table: "PlanningUserTrains",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "PlanningUserTrains",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlanningUserTrainsID",
                table: "PlanningUserRouteSaves",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "PlanningUserRouteID",
                table: "PlanningUserRouteSaves",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "TrainsShaduleID",
                table: "PlanningUserRoutes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PlanningUserRouteSaveID",
                table: "PlanningUserRoutes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainsShadule_PlanningUserRouteID",
                table: "TrainsShadule",
                column: "PlanningUserRouteID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanningUserTrains_PlanningUserRouteSaveID",
                table: "PlanningUserTrains",
                column: "PlanningUserRouteSaveID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanningUserTrains_UserId",
                table: "PlanningUserTrains",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanningUserRoutes_PlanningUserRouteSaveID",
                table: "PlanningUserRoutes",
                column: "PlanningUserRouteSaveID");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanningUserRoutes_PlanningUserRouteSaves_PlanningUserRouteSaveID",
                table: "PlanningUserRoutes",
                column: "PlanningUserRouteSaveID",
                principalTable: "PlanningUserRouteSaves",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanningUserTrains_AspNetUsers_UserId",
                table: "PlanningUserTrains",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanningUserTrains_PlanningUserRouteSaves_PlanningUserRouteSaveID",
                table: "PlanningUserTrains",
                column: "PlanningUserRouteSaveID",
                principalTable: "PlanningUserRouteSaves",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainsShadule_PlanningUserRoutes_PlanningUserRouteID",
                table: "TrainsShadule",
                column: "PlanningUserRouteID",
                principalTable: "PlanningUserRoutes",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanningUserRoutes_PlanningUserRouteSaves_PlanningUserRouteSaveID",
                table: "PlanningUserRoutes");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanningUserTrains_AspNetUsers_UserId",
                table: "PlanningUserTrains");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanningUserTrains_PlanningUserRouteSaves_PlanningUserRouteSaveID",
                table: "PlanningUserTrains");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainsShadule_PlanningUserRoutes_PlanningUserRouteID",
                table: "TrainsShadule");

            migrationBuilder.DropIndex(
                name: "IX_TrainsShadule_PlanningUserRouteID",
                table: "TrainsShadule");

            migrationBuilder.DropIndex(
                name: "IX_PlanningUserTrains_PlanningUserRouteSaveID",
                table: "PlanningUserTrains");

            migrationBuilder.DropIndex(
                name: "IX_PlanningUserTrains_UserId",
                table: "PlanningUserTrains");

            migrationBuilder.DropIndex(
                name: "IX_PlanningUserRoutes_PlanningUserRouteSaveID",
                table: "PlanningUserRoutes");

            migrationBuilder.DropColumn(
                name: "PlanningUserRouteID",
                table: "TrainsShadule");

            migrationBuilder.DropColumn(
                name: "PlanningUserRouteSaveID",
                table: "PlanningUserTrains");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PlanningUserTrains");

            migrationBuilder.DropColumn(
                name: "PlanningUserRouteSaveID",
                table: "PlanningUserRoutes");

            migrationBuilder.AlterColumn<int>(
                name: "PlanningUserTrainsID",
                table: "PlanningUserRouteSaves",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PlanningUserRouteID",
                table: "PlanningUserRouteSaves",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TrainsShaduleID",
                table: "PlanningUserRoutes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlanningUserRouteSaves_PlanningUserRouteID",
                table: "PlanningUserRouteSaves",
                column: "PlanningUserRouteID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanningUserRouteSaves_PlanningUserTrainsID",
                table: "PlanningUserRouteSaves",
                column: "PlanningUserTrainsID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanningUserRoutes_TrainsShaduleID",
                table: "PlanningUserRoutes",
                column: "TrainsShaduleID");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanningUserRoutes_TrainsShadule_TrainsShaduleID",
                table: "PlanningUserRoutes",
                column: "TrainsShaduleID",
                principalTable: "TrainsShadule",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanningUserRouteSaves_PlanningUserRoutes_PlanningUserRouteID",
                table: "PlanningUserRouteSaves",
                column: "PlanningUserRouteID",
                principalTable: "PlanningUserRoutes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanningUserRouteSaves_PlanningUserTrains_PlanningUserTrainsID",
                table: "PlanningUserRouteSaves",
                column: "PlanningUserTrainsID",
                principalTable: "PlanningUserTrains",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
