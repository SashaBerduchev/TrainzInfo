using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainzInfo.Migrations
{
    public partial class DizelTrainzListCityPower : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "DizelTrainzLists",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Power",
                table: "DizelTrainzLists",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "DizelTrainzLists");

            migrationBuilder.DropColumn(
                name: "Power",
                table: "DizelTrainzLists");
        }
    }
}
