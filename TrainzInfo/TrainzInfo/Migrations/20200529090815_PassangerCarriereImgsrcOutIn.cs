using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainzInfo.Migrations
{
    public partial class PassangerCarriereImgsrcOutIn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imgsrc",
                table: "PassangerCarrieres");

            migrationBuilder.AddColumn<string>(
                name: "ImgsrcInside",
                table: "PassangerCarrieres",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImgsrcOutside",
                table: "PassangerCarrieres",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgsrcInside",
                table: "PassangerCarrieres");

            migrationBuilder.DropColumn(
                name: "ImgsrcOutside",
                table: "PassangerCarrieres");

            migrationBuilder.AddColumn<string>(
                name: "Imgsrc",
                table: "PassangerCarrieres",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
