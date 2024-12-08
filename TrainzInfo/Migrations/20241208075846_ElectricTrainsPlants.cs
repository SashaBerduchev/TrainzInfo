using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class ElectricTrainsPlants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Electrics_Plants_Plantsid",
                table: "Electrics");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Electrics");

            migrationBuilder.RenameColumn(
                name: "Plantsid",
                table: "Electrics",
                newName: "PlantsKvrid");

            migrationBuilder.RenameColumn(
                name: "Plant",
                table: "Electrics",
                newName: "PlantKvr");

            migrationBuilder.RenameColumn(
                name: "PlaceKvr",
                table: "Electrics",
                newName: "PlantCreate");

            migrationBuilder.RenameIndex(
                name: "IX_Electrics_Plantsid",
                table: "Electrics",
                newName: "IX_Electrics_PlantsKvrid");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "LastKvr",
                table: "Electrics",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateOnly>(
                name: "CreatedTrain",
                table: "Electrics",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<int>(
                name: "PlantsCreateid",
                table: "Electrics",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Electrics_PlantsCreateid",
                table: "Electrics",
                column: "PlantsCreateid");

            migrationBuilder.AddForeignKey(
                name: "FK_Electrics_Plants_PlantsCreateid",
                table: "Electrics",
                column: "PlantsCreateid",
                principalTable: "Plants",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Electrics_Plants_PlantsKvrid",
                table: "Electrics",
                column: "PlantsKvrid",
                principalTable: "Plants",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Electrics_Plants_PlantsCreateid",
                table: "Electrics");

            migrationBuilder.DropForeignKey(
                name: "FK_Electrics_Plants_PlantsKvrid",
                table: "Electrics");

            migrationBuilder.DropIndex(
                name: "IX_Electrics_PlantsCreateid",
                table: "Electrics");

            migrationBuilder.DropColumn(
                name: "CreatedTrain",
                table: "Electrics");

            migrationBuilder.DropColumn(
                name: "PlantsCreateid",
                table: "Electrics");

            migrationBuilder.RenameColumn(
                name: "PlantsKvrid",
                table: "Electrics",
                newName: "Plantsid");

            migrationBuilder.RenameColumn(
                name: "PlantKvr",
                table: "Electrics",
                newName: "Plant");

            migrationBuilder.RenameColumn(
                name: "PlantCreate",
                table: "Electrics",
                newName: "PlaceKvr");

            migrationBuilder.RenameIndex(
                name: "IX_Electrics_PlantsKvrid",
                table: "Electrics",
                newName: "IX_Electrics_Plantsid");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastKvr",
                table: "Electrics",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Electrics",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Electrics_Plants_Plantsid",
                table: "Electrics",
                column: "Plantsid",
                principalTable: "Plants",
                principalColumn: "id");
        }
    }
}
