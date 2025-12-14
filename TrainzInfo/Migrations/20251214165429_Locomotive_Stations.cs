using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class Locomotive_Stations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Create",
                table: "Locomotives",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Stationsid",
                table: "Locomotives",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Update",
                table: "Locomotives",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Locomotives_Stationsid",
                table: "Locomotives",
                column: "Stationsid");

            migrationBuilder.AddForeignKey(
                name: "FK_Locomotives_Stations_Stationsid",
                table: "Locomotives",
                column: "Stationsid",
                principalTable: "Stations",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locomotives_Stations_Stationsid",
                table: "Locomotives");

            migrationBuilder.DropIndex(
                name: "IX_Locomotives_Stationsid",
                table: "Locomotives");

            migrationBuilder.DropColumn(
                name: "Create",
                table: "Locomotives");

            migrationBuilder.DropColumn(
                name: "Stationsid",
                table: "Locomotives");

            migrationBuilder.DropColumn(
                name: "Update",
                table: "Locomotives");
        }
    }
}
