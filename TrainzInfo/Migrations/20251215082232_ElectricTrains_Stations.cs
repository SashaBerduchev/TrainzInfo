using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class ElectricTrains_Stations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Create",
                table: "Electrics",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Stationsid",
                table: "Electrics",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Update",
                table: "Electrics",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Electrics_Stationsid",
                table: "Electrics",
                column: "Stationsid");

            migrationBuilder.AddForeignKey(
                name: "FK_Electrics_Stations_Stationsid",
                table: "Electrics",
                column: "Stationsid",
                principalTable: "Stations",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Electrics_Stations_Stationsid",
                table: "Electrics");

            migrationBuilder.DropIndex(
                name: "IX_Electrics_Stationsid",
                table: "Electrics");

            migrationBuilder.DropColumn(
                name: "Create",
                table: "Electrics");

            migrationBuilder.DropColumn(
                name: "Stationsid",
                table: "Electrics");

            migrationBuilder.DropColumn(
                name: "Update",
                table: "Electrics");
        }
    }
}
