using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class UsersFKIpAdresses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsersId",
                table: "IpAdresses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IpAdresses_UsersId",
                table: "IpAdresses",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_IpAdresses_User_UsersId",
                table: "IpAdresses",
                column: "UsersId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IpAdresses_User_UsersId",
                table: "IpAdresses");

            migrationBuilder.DropIndex(
                name: "IX_IpAdresses_UsersId",
                table: "IpAdresses");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "IpAdresses");
        }
    }
}
