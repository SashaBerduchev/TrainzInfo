using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class NewsInfoFKUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "user",
                table: "NewsInfos");

            migrationBuilder.AddColumn<int>(
                name: "UsersId",
                table: "NewsInfos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NewsInfos_UsersId",
                table: "NewsInfos",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsInfos_User_UsersId",
                table: "NewsInfos",
                column: "UsersId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsInfos_User_UsersId",
                table: "NewsInfos");

            migrationBuilder.DropIndex(
                name: "IX_NewsInfos_UsersId",
                table: "NewsInfos");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "NewsInfos");

            migrationBuilder.AddColumn<string>(
                name: "user",
                table: "NewsInfos",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
