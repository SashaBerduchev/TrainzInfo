using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainzInfo.Migrations
{
    public partial class PassangerCarriere : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PassangerCarrieges");

            migrationBuilder.CreateTable(
                name: "PassangerCarrieres",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Calss = table.Column<string>(nullable: false),
                    CountPlace = table.Column<int>(nullable: false),
                    PlaceType = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassangerCarrieres", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PassangerCarrieres");

            migrationBuilder.CreateTable(
                name: "PassangerCarrieges",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarriegeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlaceCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassangerCarrieges", x => x.id);
                });
        }
    }
}
