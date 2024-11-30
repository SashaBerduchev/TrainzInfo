using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class ElectricTrainFKPlants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Plantsid",
                table: "Electrics",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Electrics_Plantsid",
                table: "Electrics",
                column: "Plantsid");

            migrationBuilder.AddForeignKey(
                name: "FK_Electrics_plants_Plantsid",
                table: "Electrics",
                column: "Plantsid",
                principalTable: "plants",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Electrics_plants_Plantsid",
                table: "Electrics");

            migrationBuilder.DropIndex(
                name: "IX_Electrics_Plantsid",
                table: "Electrics");

            migrationBuilder.DropColumn(
                name: "Plantsid",
                table: "Electrics");
        }
    }
}
