using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class ListRollingStone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Citysid",
                table: "ListRollingStones",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DepotListid",
                table: "ListRollingStones",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ListRollingStones_Citysid",
                table: "ListRollingStones",
                column: "Citysid");

            migrationBuilder.CreateIndex(
                name: "IX_ListRollingStones_DepotListid",
                table: "ListRollingStones",
                column: "DepotListid");

            migrationBuilder.AddForeignKey(
                name: "FK_ListRollingStones_Cities_Citysid",
                table: "ListRollingStones",
                column: "Citysid",
                principalTable: "Cities",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_ListRollingStones_Depots_DepotListid",
                table: "ListRollingStones",
                column: "DepotListid",
                principalTable: "Depots",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListRollingStones_Cities_Citysid",
                table: "ListRollingStones");

            migrationBuilder.DropForeignKey(
                name: "FK_ListRollingStones_Depots_DepotListid",
                table: "ListRollingStones");

            migrationBuilder.DropIndex(
                name: "IX_ListRollingStones_Citysid",
                table: "ListRollingStones");

            migrationBuilder.DropIndex(
                name: "IX_ListRollingStones_DepotListid",
                table: "ListRollingStones");

            migrationBuilder.DropColumn(
                name: "Citysid",
                table: "ListRollingStones");

            migrationBuilder.DropColumn(
                name: "DepotListid",
                table: "ListRollingStones");
        }
    }
}
