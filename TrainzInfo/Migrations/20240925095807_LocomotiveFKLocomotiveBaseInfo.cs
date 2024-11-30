using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class LocomotiveFKLocomotiveBaseInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocomotiveBaseInfoid",
                table: "Locomotives",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locomotives_LocomotiveBaseInfoid",
                table: "Locomotives",
                column: "LocomotiveBaseInfoid");

            migrationBuilder.AddForeignKey(
                name: "FK_Locomotives_locomotiveBaseInfos_LocomotiveBaseInfoid",
                table: "Locomotives",
                column: "LocomotiveBaseInfoid",
                principalTable: "locomotiveBaseInfos",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locomotives_locomotiveBaseInfos_LocomotiveBaseInfoid",
                table: "Locomotives");

            migrationBuilder.DropIndex(
                name: "IX_Locomotives_LocomotiveBaseInfoid",
                table: "Locomotives");

            migrationBuilder.DropColumn(
                name: "LocomotiveBaseInfoid",
                table: "Locomotives");
        }
    }
}
