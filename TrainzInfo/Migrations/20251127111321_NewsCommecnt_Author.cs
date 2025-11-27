using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class NewsCommecnt_Author : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "NewsComments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NewsComments_AuthorId",
                table: "NewsComments",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsComments_AspNetUsers_AuthorId",
                table: "NewsComments",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsComments_AspNetUsers_AuthorId",
                table: "NewsComments");

            migrationBuilder.DropIndex(
                name: "IX_NewsComments_AuthorId",
                table: "NewsComments");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "NewsComments");
        }
    }
}
