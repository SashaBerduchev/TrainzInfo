using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class News_Status : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageMimeTypeOfData",
                table: "NewsInfos");

            migrationBuilder.DropColumn(
                name: "Imgsrc",
                table: "NewsInfos");

            migrationBuilder.DropColumn(
                name: "NewsImage",
                table: "NewsInfos");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateEndActual",
                table: "NewsInfos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateStartActual",
                table: "NewsInfos",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "LinkSorce",
                table: "NewsInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "NewsInfos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StatusObjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodeAlias = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusObjects", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NewsInfos_StatusId",
                table: "NewsInfos",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsInfos_StatusObjects_StatusId",
                table: "NewsInfos",
                column: "StatusId",
                principalTable: "StatusObjects",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsInfos_StatusObjects_StatusId",
                table: "NewsInfos");

            migrationBuilder.DropTable(
                name: "StatusObjects");

            migrationBuilder.DropIndex(
                name: "IX_NewsInfos_StatusId",
                table: "NewsInfos");

            migrationBuilder.DropColumn(
                name: "DateEndActual",
                table: "NewsInfos");

            migrationBuilder.DropColumn(
                name: "DateStartActual",
                table: "NewsInfos");

            migrationBuilder.DropColumn(
                name: "LinkSorce",
                table: "NewsInfos");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "NewsInfos");

            migrationBuilder.AddColumn<string>(
                name: "ImageMimeTypeOfData",
                table: "NewsInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Imgsrc",
                table: "NewsInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "NewsImage",
                table: "NewsInfos",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
