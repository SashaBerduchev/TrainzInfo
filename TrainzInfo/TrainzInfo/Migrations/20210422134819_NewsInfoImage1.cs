using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainzInfo.Migrations
{
    public partial class NewsInfoImage1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<byte[]>(
            //    name: "Image",
            //    table: "NewsInfos",
            //    nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageMimeTypeOfData",
                table: "NewsInfos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "NewsInfos");

            migrationBuilder.DropColumn(
                name: "ImageMimeTypeOfData",
                table: "NewsInfos");
        }
    }
}
