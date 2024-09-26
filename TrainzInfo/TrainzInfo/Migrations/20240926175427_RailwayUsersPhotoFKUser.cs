using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class RailwayUsersPhotoFKUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CityFrom",
                table: "RailwayUsersPhotos");

            migrationBuilder.DropColumn(
                name: "CitytTo",
                table: "RailwayUsersPhotos");

            migrationBuilder.DropColumn(
                name: "NameUser",
                table: "RailwayUsersPhotos");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RailwayUsersPhotos");

            migrationBuilder.AddColumn<int>(
                name: "UsersId",
                table: "RailwayUsersPhotos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RailwayUsersPhotos_UsersId",
                table: "RailwayUsersPhotos",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_RailwayUsersPhotos_User_UsersId",
                table: "RailwayUsersPhotos",
                column: "UsersId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RailwayUsersPhotos_User_UsersId",
                table: "RailwayUsersPhotos");

            migrationBuilder.DropIndex(
                name: "IX_RailwayUsersPhotos_UsersId",
                table: "RailwayUsersPhotos");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "RailwayUsersPhotos");

            migrationBuilder.AddColumn<string>(
                name: "CityFrom",
                table: "RailwayUsersPhotos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CitytTo",
                table: "RailwayUsersPhotos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameUser",
                table: "RailwayUsersPhotos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "RailwayUsersPhotos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
