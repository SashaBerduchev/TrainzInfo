using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainzInfo.Migrations
{
    public partial class DieselLocomoives : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DieselLocomoives",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    MaxSpeed = table.Column<int>(nullable: false),
                    SectionCount = table.Column<int>(nullable: false),
                    DiseslPower = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DieselLocomoives", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DieselLocomoives");
        }
    }
}
