using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainzInfo.Migrations
{
    public partial class userPhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoLink",
                table: "UserLocomotivePhotos");

            migrationBuilder.DropColumn(
                name: "UserSername",
                table: "UserLocomotivePhotos");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "UserLocomotivePhotos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageMimeTypeOfData",
                table: "UserLocomotivePhotos",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "UserLocomotivePhotos",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "UserLocomotivePhotos");

            migrationBuilder.DropColumn(
                name: "ImageMimeTypeOfData",
                table: "UserLocomotivePhotos");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserLocomotivePhotos");

            migrationBuilder.AddColumn<string>(
                name: "PhotoLink",
                table: "UserLocomotivePhotos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserSername",
                table: "UserLocomotivePhotos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
