using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainzInfo.Migrations
{
    public partial class NameLocomotive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameLocomotive",
                table: "UserLocomotivePhotos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameLocomotive",
                table: "UserLocomotivePhotos");
        }
    }
}
