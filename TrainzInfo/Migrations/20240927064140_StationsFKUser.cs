using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class StationsFKUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Stations");

            migrationBuilder.AddColumn<int>(
                name: "UsersId",
                table: "Stations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stations_UsersId",
                table: "Stations",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stations_User_UsersId",
                table: "Stations",
                column: "UsersId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stations_User_UsersId",
                table: "Stations");

            migrationBuilder.DropIndex(
                name: "IX_Stations_UsersId",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "Stations");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Stations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
