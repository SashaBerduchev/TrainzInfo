using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainzInfo.Migrations
{
    public partial class DopImgSrc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DopImgSrc",
                table: "Stations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DopImgSrcSec",
                table: "Stations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DopImgSrcThd",
                table: "Stations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DopImgSrc",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "DopImgSrcSec",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "DopImgSrcThd",
                table: "Stations");
        }
    }
}
