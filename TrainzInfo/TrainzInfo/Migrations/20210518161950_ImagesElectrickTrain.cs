using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainzInfo.Migrations
{
    public partial class ImagesElectrickTrain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imgsrc",
                table: "Electrics");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Electrics",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageMimeTypeOfData",
                table: "Electrics",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Electrics");

            migrationBuilder.DropColumn(
                name: "ImageMimeTypeOfData",
                table: "Electrics");

            migrationBuilder.AddColumn<string>(
                name: "Imgsrc",
                table: "Electrics",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
