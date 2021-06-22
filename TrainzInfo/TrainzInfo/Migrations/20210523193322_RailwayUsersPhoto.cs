using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainzInfo.Migrations
{
    public partial class RailwayUsersPhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RailwayUsersPhotos",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameUser = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    CityFrom = table.Column<string>(nullable: true),
                    CitytTo = table.Column<string>(nullable: true),
                    Information = table.Column<string>(nullable: true),
                    Image = table.Column<byte[]>(nullable: true),
                    ImageType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RailwayUsersPhotos", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RailwayUsersPhotos");
        }
    }
}
