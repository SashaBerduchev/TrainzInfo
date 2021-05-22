using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainzInfo.Migrations
{
    public partial class NewsImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "Image",
            //    table: "NewsInfos");

            migrationBuilder.AddColumn<byte[]>(
                name: "NewsImage",
                table: "NewsInfos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewsImage",
                table: "NewsInfos");

            //migrationBuilder.AddColumn<byte[]>(
            //    name: "Image",
            //    table: "NewsInfos",
            //    type: "varbinary(max)",
            //    nullable: true);
        }
    }
}
