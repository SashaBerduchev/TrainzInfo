using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class Train : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Trainid",
                table: "TrainsShadule",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainsShadule_Trainid",
                table: "TrainsShadule",
                column: "Trainid");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainsShadule_Trains_Trainid",
                table: "TrainsShadule",
                column: "Trainid",
                principalTable: "Trains",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainsShadule_Trains_Trainid",
                table: "TrainsShadule");

            migrationBuilder.DropIndex(
                name: "IX_TrainsShadule_Trainid",
                table: "TrainsShadule");

            migrationBuilder.DropColumn(
                name: "Trainid",
                table: "TrainsShadule");
        }
    }
}
