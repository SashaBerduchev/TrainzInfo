using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainzInfo.Migrations
{
    public partial class ListImge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photo",
                table: "ListRollingStones");

            migrationBuilder.DropColumn(
                name: "LocomotiveImg",
                table: "Electic_Locomotives");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "ListRollingStones",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageMimeTypeOfData",
                table: "ListRollingStones",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Depot",
                table: "Electic_Locomotives",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "ListRollingStones");

            migrationBuilder.DropColumn(
                name: "ImageMimeTypeOfData",
                table: "ListRollingStones");

            migrationBuilder.DropColumn(
                name: "Depot",
                table: "Electic_Locomotives");

            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "ListRollingStones",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocomotiveImg",
                table: "Electic_Locomotives",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
