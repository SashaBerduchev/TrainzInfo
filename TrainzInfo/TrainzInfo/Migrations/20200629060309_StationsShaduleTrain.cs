using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainzInfo.Migrations
{
    public partial class StationsShaduleTrain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StationsShadules",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Station = table.Column<string>(nullable: false),
                    UzFilia = table.Column<string>(nullable: false),
                    TimeOfArrive = table.Column<DateTime>(nullable: false),
                    TimeOfDepet = table.Column<DateTime>(nullable: false),
                    TrainInfo = table.Column<string>(nullable: false),
                    ImgTrain = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StationsShadules", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Trains",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StationFrom = table.Column<string>(nullable: false),
                    StationTo = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    NameOfTrain = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trains", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TypeOfPassTrains",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeOfPassTrains", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StationsShadules");

            migrationBuilder.DropTable(
                name: "Trains");

            migrationBuilder.DropTable(
                name: "TypeOfPassTrains");
        }
    }
}
