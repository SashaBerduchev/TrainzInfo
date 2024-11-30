using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class ElectricTrainFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Cityid",
                table: "Electrics",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DepotListid",
                table: "Electrics",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Electrics_Cityid",
                table: "Electrics",
                column: "Cityid");

            migrationBuilder.CreateIndex(
                name: "IX_Electrics_DepotListid",
                table: "Electrics",
                column: "DepotListid");

            migrationBuilder.AddForeignKey(
                name: "FK_Electrics_Cities_Cityid",
                table: "Electrics",
                column: "Cityid",
                principalTable: "Cities",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Electrics_Depots_DepotListid",
                table: "Electrics",
                column: "DepotListid",
                principalTable: "Depots",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Electrics_Cities_Cityid",
                table: "Electrics");

            migrationBuilder.DropForeignKey(
                name: "FK_Electrics_Depots_DepotListid",
                table: "Electrics");

            migrationBuilder.DropIndex(
                name: "IX_Electrics_Cityid",
                table: "Electrics");

            migrationBuilder.DropIndex(
                name: "IX_Electrics_DepotListid",
                table: "Electrics");

            migrationBuilder.DropColumn(
                name: "Cityid",
                table: "Electrics");

            migrationBuilder.DropColumn(
                name: "DepotListid",
                table: "Electrics");
        }
    }
}
