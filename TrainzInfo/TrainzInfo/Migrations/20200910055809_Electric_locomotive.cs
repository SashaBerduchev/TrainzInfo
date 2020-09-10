using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainzInfo.Migrations
{
    public partial class Electric_locomotive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateBuild",
                table: "Electic_Locomotives",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDelete",
                table: "Electic_Locomotives",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Seria",
                table: "Electic_Locomotives",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateBuild",
                table: "Electic_Locomotives");

            migrationBuilder.DropColumn(
                name: "DateDelete",
                table: "Electic_Locomotives");

            migrationBuilder.DropColumn(
                name: "Seria",
                table: "Electic_Locomotives");
        }
    }
}
