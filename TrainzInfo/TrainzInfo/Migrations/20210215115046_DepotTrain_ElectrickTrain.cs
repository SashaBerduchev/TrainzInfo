using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainzInfo.Migrations
{
    public partial class DepotTrain_ElectrickTrain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DepotCity",
                table: "Electrics",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepotTrain",
                table: "Electrics",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepotCity",
                table: "Electrics");

            migrationBuilder.DropColumn(
                name: "DepotTrain",
                table: "Electrics");
        }
    }
}
