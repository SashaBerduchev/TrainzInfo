using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class DocumentToIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "IpAdresses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "IpAdresses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_IpAdresses_IdentityUserId",
                table: "IpAdresses",
                column: "IdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_IpAdresses_AspNetUsers_IdentityUserId",
                table: "IpAdresses",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IpAdresses_AspNetUsers_IdentityUserId",
                table: "IpAdresses");

            migrationBuilder.DropIndex(
                name: "IX_IpAdresses_IdentityUserId",
                table: "IpAdresses");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "IpAdresses");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "IpAdresses");
        }
    }
}
