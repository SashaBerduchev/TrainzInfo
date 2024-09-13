using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class SuburbanTrainsInfoFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ElectricTrainid",
                table: "SuburbanTrainsInfos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SuburbanTrainsInfos_ElectricTrainid",
                table: "SuburbanTrainsInfos",
                column: "ElectricTrainid");

            migrationBuilder.AddForeignKey(
                name: "FK_SuburbanTrainsInfos_Electrics_ElectricTrainid",
                table: "SuburbanTrainsInfos",
                column: "ElectricTrainid",
                principalTable: "Electrics",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SuburbanTrainsInfos_Electrics_ElectricTrainid",
                table: "SuburbanTrainsInfos");

            migrationBuilder.DropIndex(
                name: "IX_SuburbanTrainsInfos_ElectricTrainid",
                table: "SuburbanTrainsInfos");

            migrationBuilder.DropColumn(
                name: "ElectricTrainid",
                table: "SuburbanTrainsInfos");
        }
    }
}
