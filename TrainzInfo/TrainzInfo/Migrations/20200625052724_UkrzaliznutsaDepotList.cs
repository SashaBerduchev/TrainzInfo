using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainzInfo.Migrations
{
    public partial class UkrzaliznutsaDepotList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Depots_UkrainsRailways_UkrainsRailwaysid",
                table: "Depots");

            migrationBuilder.DropIndex(
                name: "IX_Depots_UkrainsRailwaysid",
                table: "Depots");

            migrationBuilder.DropColumn(
                name: "UkrainsRailwaysid",
                table: "Depots");

            migrationBuilder.AddColumn<string>(
                name: "UkrainsRailways",
                table: "Depots",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UkrainsRailways",
                table: "Depots");

            migrationBuilder.AddColumn<int>(
                name: "UkrainsRailwaysid",
                table: "Depots",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Depots_UkrainsRailwaysid",
                table: "Depots",
                column: "UkrainsRailwaysid");

            migrationBuilder.AddForeignKey(
                name: "FK_Depots_UkrainsRailways_UkrainsRailwaysid",
                table: "Depots",
                column: "UkrainsRailwaysid",
                principalTable: "UkrainsRailways",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
