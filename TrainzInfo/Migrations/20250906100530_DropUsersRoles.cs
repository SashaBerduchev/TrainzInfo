using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class DropUsersRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DieselTrains_User_UsersId",
                table: "DieselTrains");

            migrationBuilder.DropForeignKey(
                name: "FK_Electrics_User_UsersId",
                table: "Electrics");

            migrationBuilder.DropForeignKey(
                name: "FK_IpAdresses_User_UsersId",
                table: "IpAdresses");

            migrationBuilder.DropForeignKey(
                name: "FK_MainImages_User_UsersId",
                table: "MainImages");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsComments_User_UsersId",
                table: "NewsComments");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsInfos_User_UsersId",
                table: "NewsInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_RailwayUsersPhotos_User_UsersId",
                table: "RailwayUsersPhotos");

            migrationBuilder.DropForeignKey(
                name: "FK_Stations_User_UsersId",
                table: "Stations");

            migrationBuilder.DropForeignKey(
                name: "FK_Trains_User_UserId",
                table: "Trains");

            migrationBuilder.DropForeignKey(
                name: "FK_UkrainsRailways_User_UsersId",
                table: "UkrainsRailways");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLocomotivePhotos_User_UserId",
                table: "UserLocomotivePhotos");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTrainzPhotos_User_UseridId",
                table: "UserTrainzPhotos");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_UserTrainzPhotos_UseridId",
                table: "UserTrainzPhotos");

            migrationBuilder.DropIndex(
                name: "IX_UserLocomotivePhotos_UserId",
                table: "UserLocomotivePhotos");

            migrationBuilder.DropIndex(
                name: "IX_UkrainsRailways_UsersId",
                table: "UkrainsRailways");

            migrationBuilder.DropIndex(
                name: "IX_Trains_UserId",
                table: "Trains");

            migrationBuilder.DropIndex(
                name: "IX_Stations_UsersId",
                table: "Stations");

            migrationBuilder.DropIndex(
                name: "IX_RailwayUsersPhotos_UsersId",
                table: "RailwayUsersPhotos");

            migrationBuilder.DropIndex(
                name: "IX_NewsInfos_UsersId",
                table: "NewsInfos");

            migrationBuilder.DropIndex(
                name: "IX_NewsComments_UsersId",
                table: "NewsComments");

            migrationBuilder.DropIndex(
                name: "IX_MainImages_UsersId",
                table: "MainImages");

            migrationBuilder.DropIndex(
                name: "IX_IpAdresses_UsersId",
                table: "IpAdresses");

            migrationBuilder.DropIndex(
                name: "IX_Electrics_UsersId",
                table: "Electrics");

            migrationBuilder.DropIndex(
                name: "IX_DieselTrains_UsersId",
                table: "DieselTrains");

            migrationBuilder.DropColumn(
                name: "UseridId",
                table: "UserTrainzPhotos");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserLocomotivePhotos");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "UkrainsRailways");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Trains");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "RailwayUsersPhotos");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "NewsInfos");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "NewsComments");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "MainImages");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "IpAdresses");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "Electrics");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "DieselTrains");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UseridId",
                table: "UserTrainzPhotos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "UserLocomotivePhotos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsersId",
                table: "UkrainsRailways",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Trains",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsersId",
                table: "Stations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsersId",
                table: "RailwayUsersPhotos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsersId",
                table: "NewsInfos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsersId",
                table: "NewsComments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsersId",
                table: "MainImages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsersId",
                table: "IpAdresses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsersId",
                table: "Electrics",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsersId",
                table: "DieselTrains",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameRole = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rules = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RolesId = table.Column<int>(type: "int", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ImageMimeTypeOfData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTrainzPhotos_UseridId",
                table: "UserTrainzPhotos",
                column: "UseridId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLocomotivePhotos_UserId",
                table: "UserLocomotivePhotos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UkrainsRailways_UsersId",
                table: "UkrainsRailways",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Trains_UserId",
                table: "Trains",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Stations_UsersId",
                table: "Stations",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_RailwayUsersPhotos_UsersId",
                table: "RailwayUsersPhotos",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsInfos_UsersId",
                table: "NewsInfos",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsComments_UsersId",
                table: "NewsComments",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_MainImages_UsersId",
                table: "MainImages",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_IpAdresses_UsersId",
                table: "IpAdresses",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Electrics_UsersId",
                table: "Electrics",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_DieselTrains_UsersId",
                table: "DieselTrains",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_User_RolesId",
                table: "User",
                column: "RolesId");

            migrationBuilder.AddForeignKey(
                name: "FK_DieselTrains_User_UsersId",
                table: "DieselTrains",
                column: "UsersId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Electrics_User_UsersId",
                table: "Electrics",
                column: "UsersId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IpAdresses_User_UsersId",
                table: "IpAdresses",
                column: "UsersId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MainImages_User_UsersId",
                table: "MainImages",
                column: "UsersId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsComments_User_UsersId",
                table: "NewsComments",
                column: "UsersId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsInfos_User_UsersId",
                table: "NewsInfos",
                column: "UsersId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RailwayUsersPhotos_User_UsersId",
                table: "RailwayUsersPhotos",
                column: "UsersId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Stations_User_UsersId",
                table: "Stations",
                column: "UsersId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trains_User_UserId",
                table: "Trains",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UkrainsRailways_User_UsersId",
                table: "UkrainsRailways",
                column: "UsersId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLocomotivePhotos_User_UserId",
                table: "UserLocomotivePhotos",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTrainzPhotos_User_UseridId",
                table: "UserTrainzPhotos",
                column: "UseridId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
