using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainzInfo.Migrations
{
    public partial class ElectricTrainDopInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Electrics",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastKvr",
                table: "Electrics",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PlaceKvr",
                table: "Electrics",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Plant",
                table: "Electrics",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "Electrics");

            migrationBuilder.DropColumn(
                name: "LastKvr",
                table: "Electrics");

            migrationBuilder.DropColumn(
                name: "PlaceKvr",
                table: "Electrics");

            migrationBuilder.DropColumn(
                name: "Plant",
                table: "Electrics");
        }
    }
}
