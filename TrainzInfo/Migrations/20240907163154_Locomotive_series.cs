using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class Locomotive_series : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Locomotive_Seriesid",
                table: "Locomotives",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locomotives_Locomotive_Seriesid",
                table: "Locomotives",
                column: "Locomotive_Seriesid");

            migrationBuilder.AddForeignKey(
                name: "FK_Locomotives_Locomotive_Series_Locomotive_Seriesid",
                table: "Locomotives",
                column: "Locomotive_Seriesid",
                principalTable: "Locomotive_Series",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locomotives_Locomotive_Series_Locomotive_Seriesid",
                table: "Locomotives");

            migrationBuilder.DropIndex(
                name: "IX_Locomotives_Locomotive_Seriesid",
                table: "Locomotives");

            migrationBuilder.DropColumn(
                name: "Locomotive_Seriesid",
                table: "Locomotives");
        }
    }
}
