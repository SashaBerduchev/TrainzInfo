using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class MainImagesFKUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainzStations");

            migrationBuilder.DropTable(
                name: "TrainzTypes");

            migrationBuilder.AddColumn<int>(
                name: "UsersId",
                table: "MainImages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MainImages_UsersId",
                table: "MainImages",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_MainImages_User_UsersId",
                table: "MainImages",
                column: "UsersId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MainImages_User_UsersId",
                table: "MainImages");

            migrationBuilder.DropIndex(
                name: "IX_MainImages_UsersId",
                table: "MainImages");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "MainImages");

            migrationBuilder.CreateTable(
                name: "TrainzStations",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameStationStop = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOFTrain = table.Column<int>(type: "int", nullable: false),
                    TimeOfArrive = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeOfDepet = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainzStations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TrainzTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainzTypes", x => x.Id);
                });
        }
    }
}
