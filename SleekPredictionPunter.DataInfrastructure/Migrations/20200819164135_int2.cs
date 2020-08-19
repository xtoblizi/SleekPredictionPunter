using Microsoft.EntityFrameworkCore.Migrations;

namespace SleekPredictionPunter.DataInfrastructure.Migrations
{
    public partial class int2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClubAScore",
                table: "Predictions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClubBScore",
                table: "Predictions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClubAScore",
                table: "Predictions");

            migrationBuilder.DropColumn(
                name: "ClubBScore",
                table: "Predictions");
        }
    }
}
