using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class UkrainsRailwaysFKUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsersId",
                table: "UkrainsRailways",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UkrainsRailways_UsersId",
                table: "UkrainsRailways",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_UkrainsRailways_User_UsersId",
                table: "UkrainsRailways",
                column: "UsersId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UkrainsRailways_User_UsersId",
                table: "UkrainsRailways");

            migrationBuilder.DropIndex(
                name: "IX_UkrainsRailways_UsersId",
                table: "UkrainsRailways");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "UkrainsRailways");
        }
    }
}
