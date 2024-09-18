using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class UserLocoPhotoFKPhotoFKUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "UserLocomotivePhotos");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UserLocomotivePhotos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Locomotiveid",
                table: "UserLocomotivePhotos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserLocomotivePhotos_Locomotiveid",
                table: "UserLocomotivePhotos",
                column: "Locomotiveid");

            migrationBuilder.CreateIndex(
                name: "IX_UserLocomotivePhotos_UserId",
                table: "UserLocomotivePhotos",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLocomotivePhotos_Locomotives_Locomotiveid",
                table: "UserLocomotivePhotos",
                column: "Locomotiveid",
                principalTable: "Locomotives",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLocomotivePhotos_User_UserId",
                table: "UserLocomotivePhotos",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLocomotivePhotos_Locomotives_Locomotiveid",
                table: "UserLocomotivePhotos");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLocomotivePhotos_User_UserId",
                table: "UserLocomotivePhotos");

            migrationBuilder.DropIndex(
                name: "IX_UserLocomotivePhotos_Locomotiveid",
                table: "UserLocomotivePhotos");

            migrationBuilder.DropIndex(
                name: "IX_UserLocomotivePhotos_UserId",
                table: "UserLocomotivePhotos");

            migrationBuilder.DropColumn(
                name: "Locomotiveid",
                table: "UserLocomotivePhotos");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UserLocomotivePhotos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "UserLocomotivePhotos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
