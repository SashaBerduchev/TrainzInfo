using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class TrainShaduller : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Stationsid",
                table: "TrainsShadule",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainsShadule_Stationsid",
                table: "TrainsShadule",
                column: "Stationsid");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainsShadule_Stations_Stationsid",
                table: "TrainsShadule",
                column: "Stationsid",
                principalTable: "Stations",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
