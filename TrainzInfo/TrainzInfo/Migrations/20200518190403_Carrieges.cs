using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainzInfo.Migrations
{
    public partial class Carrieges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_trainzTypes",
                table: "trainzTypes");

            migrationBuilder.RenameTable(
                name: "trainzTypes",
                newName: "TrainzTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrainzTypes",
                table: "TrainzTypes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CargoCarrieges",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarriegeType = table.Column<string>(nullable: false),
                    MaxSpeed = table.Column<int>(nullable: false),
                    CargoType = table.Column<string>(nullable: false),
                    CargoWeight = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CargoCarrieges", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PassangerCarrieges",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarriegeType = table.Column<string>(nullable: false),
                    PlaceCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassangerCarrieges", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CargoCarrieges");

            migrationBuilder.DropTable(
                name: "PassangerCarrieges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TrainzTypes",
                table: "TrainzTypes");

            migrationBuilder.RenameTable(
                name: "TrainzTypes",
                newName: "trainzTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_trainzTypes",
                table: "trainzTypes",
                column: "Id");
        }
    }
}
