using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class ElectricTrainFKUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "User",
                table: "Electrics");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Electrics");

            migrationBuilder.AddColumn<int>(
                name: "UsersId",
                table: "Electrics",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Electrics_UsersId",
                table: "Electrics",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Electrics_User_UsersId",
                table: "Electrics",
                column: "UsersId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Electrics_User_UsersId",
                table: "Electrics");

            migrationBuilder.DropIndex(
                name: "IX_Electrics_UsersId",
                table: "Electrics");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "Electrics");

            migrationBuilder.AddColumn<string>(
                name: "User",
                table: "Electrics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Electrics",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
