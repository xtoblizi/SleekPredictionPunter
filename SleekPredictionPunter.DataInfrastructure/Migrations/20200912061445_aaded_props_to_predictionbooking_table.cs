using Microsoft.EntityFrameworkCore.Migrations;

namespace SleekPredictionPunter.DataInfrastructure.Migrations
{
    public partial class aaded_props_to_predictionbooking_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DisplayonHome",
                table: "PredictionBookings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PredictionResult",
                table: "PredictionBookings",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayonHome",
                table: "PredictionBookings");

            migrationBuilder.DropColumn(
                name: "PredictionResult",
                table: "PredictionBookings");
        }
    }
}
