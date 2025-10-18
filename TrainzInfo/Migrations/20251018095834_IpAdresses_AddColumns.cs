using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class IpAdresses_AddColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
            name: "IpAdresses",
            columns: table => new
            {
                id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                IpAddres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                DateCreate = table.Column<DateTime>(type: "datetime2", nullable: false),
                DateUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_IpAdresses", x => x.id);
            });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
