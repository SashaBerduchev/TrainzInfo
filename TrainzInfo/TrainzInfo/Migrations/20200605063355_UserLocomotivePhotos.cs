using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainzInfo.Migrations
{
    public partial class UserLocomotivePhotos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserLocomotivePhotos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(nullable: false),
                    UserSername = table.Column<string>(nullable: false),
                    BaseInfo = table.Column<string>(nullable: false),
                    AllInfo = table.Column<string>(nullable: false),
                    PhotoLink = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLocomotivePhotos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserLocomotivePhotos");
        }
    }
}
