using Microsoft.EntityFrameworkCore.Migrations;

namespace SleekPredictionPunter.DataInfrastructure.Migrations
{
    public partial class addedboolflag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SetforHomePreview",
                table: "WinningPlanPreviewSummary",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SetforHomePreview",
                table: "WinningPlanPreviewSummary");
        }
    }
}
