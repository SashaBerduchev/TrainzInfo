using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class NewsImage_NewsInfoId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsInfos_NewsImages_NewsImagesid",
                table: "NewsInfos");

            migrationBuilder.DropIndex(
                name: "IX_NewsInfos_NewsImagesid",
                table: "NewsInfos");

            migrationBuilder.DropColumn(
                name: "NewsImagesid",
                table: "NewsInfos");

            migrationBuilder.AddColumn<int>(
                name: "NewsInfoId",
                table: "NewsImages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_NewsImages_NewsInfoId",
                table: "NewsImages",
                column: "NewsInfoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsImages_NewsInfos_NewsInfoId",
                table: "NewsImages",
                column: "NewsInfoId",
                principalTable: "NewsInfos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsImages_NewsInfos_NewsInfoId",
                table: "NewsImages");

            migrationBuilder.DropIndex(
                name: "IX_NewsImages_NewsInfoId",
                table: "NewsImages");

            migrationBuilder.DropColumn(
                name: "NewsInfoId",
                table: "NewsImages");

            migrationBuilder.AddColumn<int>(
                name: "NewsImagesid",
                table: "NewsInfos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NewsInfos_NewsImagesid",
                table: "NewsInfos",
                column: "NewsImagesid");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsInfos_NewsImages_NewsImagesid",
                table: "NewsInfos",
                column: "NewsImagesid",
                principalTable: "NewsImages",
                principalColumn: "id");
        }
    }
}
