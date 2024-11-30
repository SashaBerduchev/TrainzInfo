using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class City : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Oblastsid",
                table: "Cities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cities_Oblastsid",
                table: "Cities",
                column: "Oblastsid");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Oblasts_Oblastsid",
                table: "Cities",
                column: "Oblastsid",
                principalTable: "Oblasts",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Oblasts_Oblastsid",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Cities_Oblastsid",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "Oblastsid",
                table: "Cities");
        }
    }
}
