using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainzInfo.Migrations
{
    public partial class DDiesel_trainzInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AllInfo",
                table: "Diesel_Trinzs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BaseInfo",
                table: "Diesel_Trinzs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllInfo",
                table: "Diesel_Trinzs");

            migrationBuilder.DropColumn(
                name: "BaseInfo",
                table: "Diesel_Trinzs");
        }
    }
}
