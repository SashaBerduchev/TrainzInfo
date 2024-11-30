using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class TrainStatuinsUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Fromid",
                table: "Trains",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Toid",
                table: "Trains",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Trains",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trains_Fromid",
                table: "Trains",
                column: "Fromid");

            migrationBuilder.CreateIndex(
                name: "IX_Trains_Toid",
                table: "Trains",
                column: "Toid");

            migrationBuilder.CreateIndex(
                name: "IX_Trains_UserId",
                table: "Trains",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trains_Stations_Fromid",
                table: "Trains",
                column: "Fromid",
                principalTable: "Stations",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trains_Stations_Toid",
                table: "Trains",
                column: "Toid",
                principalTable: "Stations",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trains_User_UserId",
                table: "Trains",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trains_Stations_Fromid",
                table: "Trains");

            migrationBuilder.DropForeignKey(
                name: "FK_Trains_Stations_Toid",
                table: "Trains");

            migrationBuilder.DropForeignKey(
                name: "FK_Trains_User_UserId",
                table: "Trains");

            migrationBuilder.DropIndex(
                name: "IX_Trains_Fromid",
                table: "Trains");

            migrationBuilder.DropIndex(
                name: "IX_Trains_Toid",
                table: "Trains");

            migrationBuilder.DropIndex(
                name: "IX_Trains_UserId",
                table: "Trains");

            migrationBuilder.DropColumn(
                name: "Fromid",
                table: "Trains");

            migrationBuilder.DropColumn(
                name: "Toid",
                table: "Trains");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Trains");
        }
    }
}
