using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class DropPassTrainsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PassangerCarriegesInfos");

            migrationBuilder.DropTable(
                name: "PassangerCarrieres");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PassangerCarriegesInfos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Info = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassangerCarriegesInfos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PassangerCarrieres",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Calss = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountPlace = table.Column<int>(type: "int", nullable: false),
                    ImgsrcInside = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImgsrcOutside = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlaceType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassangerCarrieres", x => x.id);
                });
        }
    }
}
