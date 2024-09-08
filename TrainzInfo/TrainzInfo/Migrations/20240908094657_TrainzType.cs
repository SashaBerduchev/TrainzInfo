using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class TrainzType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrainzTypeId",
                table: "Trains",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trains_TrainzTypeId",
                table: "Trains",
                column: "TrainzTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trains_TrainzTypes_TrainzTypeId",
                table: "Trains",
                column: "TrainzTypeId",
                principalTable: "TrainzTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trains_TrainzTypes_TrainzTypeId",
                table: "Trains");

            migrationBuilder.DropIndex(
                name: "IX_Trains_TrainzTypeId",
                table: "Trains");

            migrationBuilder.DropColumn(
                name: "TrainzTypeId",
                table: "Trains");
        }
    }
}
