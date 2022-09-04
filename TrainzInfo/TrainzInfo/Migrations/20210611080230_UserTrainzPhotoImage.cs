using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainzInfo.Migrations
{
    public partial class UserTrainzPhotoImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imgsrc",
                table: "UserTrainzPhotos");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "UserTrainzPhotos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageType",
                table: "UserTrainzPhotos",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "UserTrainzPhotos",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "UserTrainzPhotos");

            migrationBuilder.DropColumn(
                name: "ImageType",
                table: "UserTrainzPhotos");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserTrainzPhotos");

            migrationBuilder.AddColumn<string>(
                name: "Imgsrc",
                table: "UserTrainzPhotos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
