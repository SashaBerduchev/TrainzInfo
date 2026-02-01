using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class PlanningUserRouteSave_Depeat_Arrival : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Arrive",
                table: "PlanningUserRouteSaves",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Depeat",
                table: "PlanningUserRouteSaves",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Arrive",
                table: "PlanningUserRouteSaves");

            migrationBuilder.DropColumn(
                name: "Depeat",
                table: "PlanningUserRouteSaves");
        }
    }
}
