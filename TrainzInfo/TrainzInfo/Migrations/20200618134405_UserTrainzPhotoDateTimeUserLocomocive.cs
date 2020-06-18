using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainzInfo.Migrations
{
    public partial class UserTrainzPhotoDateTimeUserLocomocive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LocmotiveName",
                table: "UserTrainzPhotos",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "UserLocomotivePhotos",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocmotiveName",
                table: "UserTrainzPhotos");

            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "UserLocomotivePhotos");
        }
    }
}
