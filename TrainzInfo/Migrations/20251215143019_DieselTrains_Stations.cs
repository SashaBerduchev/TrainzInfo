using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class DieselTrains_Stations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Create",
                table: "DieselTrains",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Stationsid",
                table: "DieselTrains",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Update",
                table: "DieselTrains",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_DieselTrains_Stationsid",
                table: "DieselTrains",
                column: "Stationsid");

            migrationBuilder.AddForeignKey(
                name: "FK_DieselTrains_Stations_Stationsid",
                table: "DieselTrains",
                column: "Stationsid",
                principalTable: "Stations",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DieselTrains_Stations_Stationsid",
                table: "DieselTrains");

            migrationBuilder.DropIndex(
                name: "IX_DieselTrains_Stationsid",
                table: "DieselTrains");

            migrationBuilder.DropColumn(
                name: "Create",
                table: "DieselTrains");

            migrationBuilder.DropColumn(
                name: "Stationsid",
                table: "DieselTrains");

            migrationBuilder.DropColumn(
                name: "Update",
                table: "DieselTrains");
        }
    }
}
