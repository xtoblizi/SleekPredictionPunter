using Microsoft.EntityFrameworkCore.Migrations;

namespace SleekPredictionPunter.DataInfrastructure.Migrations
{
    public partial class betcategory_field_inprediction_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BeCategory",
                table: "Predictions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BeCategory",
                table: "Predictions");
        }
    }
}
