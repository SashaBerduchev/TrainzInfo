using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainzInfo.Migrations
{
    public partial class DepotList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UkrainsRailways",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Information = table.Column<string>(nullable: false),
                    Photo = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UkrainsRailways", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Depots",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    UkrainsRailwaysid = table.Column<int>(nullable: false),
                    Addres = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Depots", x => x.id);
                    table.ForeignKey(
                        name: "FK_Depots_UkrainsRailways_UkrainsRailwaysid",
                        column: x => x.UkrainsRailwaysid,
                        principalTable: "UkrainsRailways",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Depots_UkrainsRailwaysid",
                table: "Depots",
                column: "UkrainsRailwaysid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Depots");

            migrationBuilder.DropTable(
                name: "UkrainsRailways");
        }
    }
}
