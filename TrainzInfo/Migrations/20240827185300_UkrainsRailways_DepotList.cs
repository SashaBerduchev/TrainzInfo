using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class UkrainsRailways_DepotList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "Locomotivess");

            migrationBuilder.AddColumn<int>(
                name: "UkrainsRailwayid",
                table: "Depots",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Depots_UkrainsRailwayid",
                table: "Depots",
                column: "UkrainsRailwayid");

            migrationBuilder.AddForeignKey(
                name: "FK_Depots_UkrainsRailways_UkrainsRailwayid",
                table: "Depots",
                column: "UkrainsRailwayid",
                principalTable: "UkrainsRailways",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Depots_UkrainsRailways_UkrainsRailwayid",
                table: "Depots");

            migrationBuilder.DropIndex(
                name: "IX_Depots_UkrainsRailwayid",
                table: "Depots");

            migrationBuilder.DropColumn(
                name: "UkrainsRailwayid",
                table: "Depots");

            migrationBuilder.CreateTable(
                name: "Locomotivess",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ALlPowerP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Depot = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DieselPower = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ImageMimeTypeOfData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SectionCount = table.Column<int>(type: "int", nullable: false),
                    Seria = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Speed = table.Column<int>(type: "int", nullable: false),
                    User = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locomotivess", x => x.id);
                });
        }
    }
}
