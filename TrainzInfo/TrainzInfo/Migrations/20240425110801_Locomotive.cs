using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class Locomotive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Locomotives",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Speed = table.Column<int>(type: "int", nullable: false),
                    SectionCount = table.Column<int>(type: "int", nullable: false),
                    ALlPowerP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Seria = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Depot = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ImageMimeTypeOfData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DieselPower = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locomotives", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Locomotives");
        }
    }
}
