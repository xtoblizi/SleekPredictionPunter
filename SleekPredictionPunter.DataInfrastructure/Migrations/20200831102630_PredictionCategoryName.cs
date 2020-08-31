using Microsoft.EntityFrameworkCore.Migrations;

namespace SleekPredictionPunter.DataInfrastructure.Migrations
{
    public partial class PredictionCategoryName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PredictionCategoryName",
                table: "Predictions",
                nullable: true,
                defaultValue: "Not Set");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PredictionCategoryName",
                table: "Predictions");
        }
    }
}
