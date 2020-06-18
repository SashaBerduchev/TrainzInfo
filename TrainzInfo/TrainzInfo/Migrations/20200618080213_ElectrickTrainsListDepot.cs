using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainzInfo.Migrations
{
    public partial class ElectrickTrainsListDepot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Depo",
                table: "Electrics");

            migrationBuilder.AddColumn<string>(
                name: "Depo",
                table: "ElectrickTrainsList",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Depo",
                table: "ElectrickTrainsList");

            migrationBuilder.AddColumn<string>(
                name: "Depo",
                table: "Electrics",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
