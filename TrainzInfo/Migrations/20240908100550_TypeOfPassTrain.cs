using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class TypeOfPassTrain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trains_TrainzTypes_TrainzTypeId",
                table: "Trains");

            migrationBuilder.RenameColumn(
                name: "TrainzTypeId",
                table: "Trains",
                newName: "TypeOfPassTrainid");

            migrationBuilder.RenameIndex(
                name: "IX_Trains_TrainzTypeId",
                table: "Trains",
                newName: "IX_Trains_TypeOfPassTrainid");

            migrationBuilder.AddForeignKey(
                name: "FK_Trains_TypeOfPassTrains_TypeOfPassTrainid",
                table: "Trains",
                column: "TypeOfPassTrainid",
                principalTable: "TypeOfPassTrains",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trains_TypeOfPassTrains_TypeOfPassTrainid",
                table: "Trains");

            migrationBuilder.RenameColumn(
                name: "TypeOfPassTrainid",
                table: "Trains",
                newName: "TrainzTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Trains_TypeOfPassTrainid",
                table: "Trains",
                newName: "IX_Trains_TrainzTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trains_TrainzTypes_TrainzTypeId",
                table: "Trains",
                column: "TrainzTypeId",
                principalTable: "TrainzTypes",
                principalColumn: "Id");
        }
    }
}
