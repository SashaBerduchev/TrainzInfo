using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class DropDieselTrains : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DieselLocomoives");

            migrationBuilder.DropTable(
                name: "DieselLocomotiveInfos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DieselLocomoives",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiseslPower = table.Column<int>(type: "int", nullable: false),
                    Imgsrc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxSpeed = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SectionCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DieselLocomoives", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DieselLocomotiveInfos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AllInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Baseinfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Diesel_Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Imgsrc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Power = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DieselLocomotiveInfos", x => x.id);
                });
        }
    }
}
