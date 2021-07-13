using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainzInfo.Migrations
{
    public partial class ALlPowerPstring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Power",
                table: "Electrick_Lockomotive_Infos",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Diesel",
                table: "Electrick_Lockomotive_Infos",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ALlPowerP",
                table: "Electic_Locomotives",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Diesel",
                table: "Electrick_Lockomotive_Infos");

            migrationBuilder.AlterColumn<int>(
                name: "Power",
                table: "Electrick_Lockomotive_Infos",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ALlPowerP",
                table: "Electic_Locomotives",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
