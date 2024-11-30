using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class NewsCommentsFKNewsInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NewsInfoid",
                table: "NewsComments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsersId",
                table: "NewsComments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NewsComments_NewsInfoid",
                table: "NewsComments",
                column: "NewsInfoid");

            migrationBuilder.CreateIndex(
                name: "IX_NewsComments_UsersId",
                table: "NewsComments",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsComments_NewsInfos_NewsInfoid",
                table: "NewsComments",
                column: "NewsInfoid",
                principalTable: "NewsInfos",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsComments_User_UsersId",
                table: "NewsComments",
                column: "UsersId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsComments_NewsInfos_NewsInfoid",
                table: "NewsComments");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsComments_User_UsersId",
                table: "NewsComments");

            migrationBuilder.DropIndex(
                name: "IX_NewsComments_NewsInfoid",
                table: "NewsComments");

            migrationBuilder.DropIndex(
                name: "IX_NewsComments_UsersId",
                table: "NewsComments");

            migrationBuilder.DropColumn(
                name: "NewsInfoid",
                table: "NewsComments");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "NewsComments");
        }
    }
}
