using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class NewsImags_News : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NewsImagesid",
                table: "NewsInfos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "NewsImages",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ImageMimeTypeOfData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsImages", x => x.id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsInfos_NewsImages_NewsImagesid",
                table: "NewsInfos");

            migrationBuilder.DropTable(
                name: "NewsImages");

            migrationBuilder.DropIndex(
                name: "IX_NewsInfos_NewsImagesid",
                table: "NewsInfos");

            migrationBuilder.DropColumn(
                name: "NewsImagesid",
                table: "NewsInfos");
        }
    }
}
