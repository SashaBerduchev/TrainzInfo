using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainzInfo.Migrations
{
    public partial class UsersImg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageMimeTypeOfData",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ImageMimeTypeOfData",
                table: "User");
        }
    }
}
