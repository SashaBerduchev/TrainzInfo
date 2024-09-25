using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class ElectrickTrainsListFKElectrickTrainzInformation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ElectrickTrainzInformationid",
                table: "ElectrickTrainsList",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ElectrickTrainsList_ElectrickTrainzInformationid",
                table: "ElectrickTrainsList",
                column: "ElectrickTrainzInformationid");

            migrationBuilder.AddForeignKey(
                name: "FK_ElectrickTrainsList_ElectrickTrainzInformation_ElectrickTrainzInformationid",
                table: "ElectrickTrainsList",
                column: "ElectrickTrainzInformationid",
                principalTable: "ElectrickTrainzInformation",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ElectrickTrainsList_ElectrickTrainzInformation_ElectrickTrainzInformationid",
                table: "ElectrickTrainsList");

            migrationBuilder.DropIndex(
                name: "IX_ElectrickTrainsList_ElectrickTrainzInformationid",
                table: "ElectrickTrainsList");

            migrationBuilder.DropColumn(
                name: "ElectrickTrainzInformationid",
                table: "ElectrickTrainsList");
        }
    }
}
