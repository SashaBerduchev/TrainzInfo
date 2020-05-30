using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainzInfo.Migrations
{
    public partial class UserPasswordStrStatusUSer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsInfos_Users_userId",
                table: "NewsInfos");

            migrationBuilder.DropIndex(
                name: "IX_NewsInfos_userId",
                table: "NewsInfos");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "NewsInfos");

            migrationBuilder.AddColumn<string>(
                name: "user",
                table: "NewsInfos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "user",
                table: "NewsInfos");

            migrationBuilder.AddColumn<int>(
                name: "userId",
                table: "NewsInfos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NewsInfos_userId",
                table: "NewsInfos",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsInfos_Users_userId",
                table: "NewsInfos",
                column: "userId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
