using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class DropTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CargoCarrieges");

            migrationBuilder.DropTable(
                name: "CargoCarriegesInfos");

            migrationBuilder.DropTable(
                name: "Diesel_Train_Infos");

            migrationBuilder.DropTable(
                name: "Diesel_Trinzs");

            migrationBuilder.DropTable(
                name: "Electrick_Lockomotive_Infos");

            migrationBuilder.DropTable(
                name: "ListRollingStones");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropColumn(
                name: "ALlPowerP",
                table: "Locomotives");

            migrationBuilder.DropColumn(
                name: "DieselPower",
                table: "Locomotives");

            migrationBuilder.DropColumn(
                name: "SectionCount",
                table: "Locomotives");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Locomotives");

            //migrationBuilder.AddColumn<int>(
            //    name: "TypeOfPassTrainid",
            //    table: "Trains",
            //    type: "int",
            //    nullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Trains_TypeOfPassTrainid",
            //    table: "Trains",
            //    column: "TypeOfPassTrainid");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Trains_TypeOfPassTrains_TypeOfPassTrainid",
            //    table: "Trains",
            //    column: "TypeOfPassTrainid",
            //    principalTable: "TypeOfPassTrains",
            //    principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trains_TypeOfPassTrains_TypeOfPassTrainid",
                table: "Trains");

            migrationBuilder.DropIndex(
                name: "IX_Trains_TypeOfPassTrainid",
                table: "Trains");

            migrationBuilder.DropColumn(
                name: "TypeOfPassTrainid",
                table: "Trains");

            migrationBuilder.AddColumn<string>(
                name: "ALlPowerP",
                table: "Locomotives",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DieselPower",
                table: "Locomotives",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SectionCount",
                table: "Locomotives",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Locomotives",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CargoCarrieges",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CargoType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CargoWeight = table.Column<int>(type: "int", nullable: false),
                    CarriegeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Imgsrc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxSpeed = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CargoCarrieges", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "CargoCarriegesInfos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Imgsrc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Info = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CargoCarriegesInfos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Diesel_Train_Infos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AllInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BaseInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Imgsrc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Power = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diesel_Train_Infos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Diesel_Trinzs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Depot = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImgSrc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VagonCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diesel_Trinzs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Electrick_Lockomotive_Infos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AllInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Baseinfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Diesel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Electric_Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Power = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Electrick_Lockomotive_Infos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ListRollingStones",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Citysid = table.Column<int>(type: "int", nullable: true),
                    DepotListid = table.Column<int>(type: "int", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Depot = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ImageMimeTypeOfData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListRollingStones", x => x.id);
                    table.ForeignKey(
                        name: "FK_ListRollingStones_Cities_Citysid",
                        column: x => x.Citysid,
                        principalTable: "Cities",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ListRollingStones_Depots_DepotListid",
                        column: x => x.DepotListid,
                        principalTable: "Depots",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status_namr = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListRollingStones_Citysid",
                table: "ListRollingStones",
                column: "Citysid");

            migrationBuilder.CreateIndex(
                name: "IX_ListRollingStones_DepotListid",
                table: "ListRollingStones",
                column: "DepotListid");
        }
    }
}
