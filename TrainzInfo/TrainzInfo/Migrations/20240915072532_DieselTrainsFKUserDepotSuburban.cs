using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class DieselTrainsFKUserDepotSuburban : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Cityid",
                table: "Depots",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DieselTrains",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SuburbanTrainsInfoid = table.Column<int>(type: "int", nullable: true),
                    NumberTrain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepotListid = table.Column<int>(type: "int", nullable: true),
                    UsersId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DieselTrains", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DieselTrains_Depots_DepotListid",
                        column: x => x.DepotListid,
                        principalTable: "Depots",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DieselTrains_SuburbanTrainsInfos_SuburbanTrainsInfoid",
                        column: x => x.SuburbanTrainsInfoid,
                        principalTable: "SuburbanTrainsInfos",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DieselTrains_User_UsersId",
                        column: x => x.UsersId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Depots_Cityid",
                table: "Depots",
                column: "Cityid");

            migrationBuilder.CreateIndex(
                name: "IX_DieselTrains_DepotListid",
                table: "DieselTrains",
                column: "DepotListid");

            migrationBuilder.CreateIndex(
                name: "IX_DieselTrains_SuburbanTrainsInfoid",
                table: "DieselTrains",
                column: "SuburbanTrainsInfoid");

            migrationBuilder.CreateIndex(
                name: "IX_DieselTrains_UsersId",
                table: "DieselTrains",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Depots_Cities_Cityid",
                table: "Depots",
                column: "Cityid",
                principalTable: "Cities",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Depots_Cities_Cityid",
                table: "Depots");

            migrationBuilder.DropTable(
                name: "DieselTrains");

            migrationBuilder.DropIndex(
                name: "IX_Depots_Cityid",
                table: "Depots");

            migrationBuilder.DropColumn(
                name: "Cityid",
                table: "Depots");
        }
    }
}
