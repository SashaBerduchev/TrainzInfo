using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class PlanningUserRouteSave_Drop_RouteID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlanningUserRouteID",
                table: "PlanningUserRouteSaves");

            migrationBuilder.DropColumn(
                name: "PlanningUserTrainsID",
                table: "PlanningUserRouteSaves");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PlanningUserRouteID",
                table: "PlanningUserRouteSaves",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlanningUserTrainsID",
                table: "PlanningUserRouteSaves",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
