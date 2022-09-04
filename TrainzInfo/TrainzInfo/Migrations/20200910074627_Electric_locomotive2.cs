using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainzInfo.Migrations
{
    public partial class Electric_locomotive2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Electic_Locomotives");

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Electic_Locomotives",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "Electic_Locomotives");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Electic_Locomotives",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
