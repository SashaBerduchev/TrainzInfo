using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class LocomotiveFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepotListid",
                table: "Locomotives",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locomotives_DepotListid",
                table: "Locomotives",
                column: "DepotListid");

            migrationBuilder.AddForeignKey(
                name: "FK_Locomotives_Depots_DepotListid",
                table: "Locomotives",
                column: "DepotListid",
                principalTable: "Depots",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locomotives_Depots_DepotListid",
                table: "Locomotives");

            migrationBuilder.DropIndex(
                name: "IX_Locomotives_DepotListid",
                table: "Locomotives");

            migrationBuilder.DropColumn(
                name: "DepotListid",
                table: "Locomotives");
        }
    }
}
