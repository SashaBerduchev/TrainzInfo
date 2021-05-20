using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainzInfo.Migrations
{
    public partial class ElectrickLocImg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Electic_Locomotives",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageMimeTypeOfData",
                table: "Electic_Locomotives",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Electic_Locomotives",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Electic_Locomotives");

            migrationBuilder.DropColumn(
                name: "ImageMimeTypeOfData",
                table: "Electic_Locomotives");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Electic_Locomotives");
        }
    }
}
