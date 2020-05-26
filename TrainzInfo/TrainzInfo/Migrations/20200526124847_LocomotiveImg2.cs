using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainzInfo.Migrations
{
    public partial class LocomotiveImg2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterTable(
               //name: "Electic_Locomotives",
               //columns: table => new
               //{
               //    id = table.Column<int>(nullable: false)
               //        .Annotation("SqlServer:Identity", "1, 1"),
               //    Name = table.Column<string>(nullable: true),
               //    Speed = table.Column<int>(nullable: false),
               //    SectionCount = table.Column<int>(nullable: false),
               //    ALlPowerP = table.Column<int>(nullable: false),
               //    LocomotiveImg = table.Column<string>(nullable: false)
               //},
               //constraints: table =>
               //{
               //    table.PrimaryKey("PK_Electic_Locomotives", x => x.id);
               //});
            migrationBuilder.AddColumn<string>(
               name: "LocomotiveImg",
               table: "Electic_Locomotives",
               nullable: false,
               defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Electic_Locomotives");
        }
    }
}
