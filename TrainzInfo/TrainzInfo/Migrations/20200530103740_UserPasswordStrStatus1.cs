using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainzInfo.Migrations
{
    public partial class UserPasswordStrStatus1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "userId",
                table: "NewsInfos",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
