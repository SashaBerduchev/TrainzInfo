using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainzInfo.Migrations
{
    /// <inheritdoc />
    public partial class TrainIsUsing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUsing",
                table: "TrainsShadule",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsUsing",
                table: "Trains",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsUsing",
                table: "StationsShadules",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUsing",
                table: "TrainsShadule");

            migrationBuilder.DropColumn(
                name: "IsUsing",
                table: "Trains");

            migrationBuilder.DropColumn(
                name: "IsUsing",
                table: "StationsShadules");
        }
    }
}
