using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class ElectrickTrainzInformationFKElectricTrain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ElectrickTrainsList");

            migrationBuilder.AddColumn<int>(
                name: "ElectrickTrainzInformationid",
                table: "Electrics",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Electrics_ElectrickTrainzInformationid",
                table: "Electrics",
                column: "ElectrickTrainzInformationid");

            migrationBuilder.AddForeignKey(
                name: "FK_Electrics_ElectrickTrainzInformation_ElectrickTrainzInformationid",
                table: "Electrics",
                column: "ElectrickTrainzInformationid",
                principalTable: "ElectrickTrainzInformation",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Electrics_ElectrickTrainzInformation_ElectrickTrainzInformationid",
                table: "Electrics");

            migrationBuilder.DropIndex(
                name: "IX_Electrics_ElectrickTrainzInformationid",
                table: "Electrics");

            migrationBuilder.DropColumn(
                name: "ElectrickTrainzInformationid",
                table: "Electrics");

            migrationBuilder.CreateTable(
                name: "ElectrickTrainsList",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ElectrickTrainzInformationid = table.Column<int>(type: "int", nullable: true),
                    Depo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Imgsrc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberTrain = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectrickTrainsList", x => x.id);
                    table.ForeignKey(
                        name: "FK_ElectrickTrainsList_ElectrickTrainzInformation_ElectrickTrainzInformationid",
                        column: x => x.ElectrickTrainzInformationid,
                        principalTable: "ElectrickTrainzInformation",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ElectrickTrainsList_ElectrickTrainzInformationid",
                table: "ElectrickTrainsList",
                column: "ElectrickTrainzInformationid");
        }
    }
}
