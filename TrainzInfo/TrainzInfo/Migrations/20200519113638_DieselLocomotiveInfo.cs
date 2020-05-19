using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainzInfo.Migrations
{
    public partial class DieselLocomotiveInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DieselLocomotiveInfos",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Diesel_Type = table.Column<string>(nullable: true),
                    Power = table.Column<int>(nullable: false),
                    Baseinfo = table.Column<string>(nullable: true),
                    AllInfo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DieselLocomotiveInfos", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DieselLocomotiveInfos");
        }
    }
}
