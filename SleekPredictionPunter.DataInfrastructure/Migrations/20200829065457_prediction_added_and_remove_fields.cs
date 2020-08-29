using Microsoft.EntityFrameworkCore.Migrations;

namespace SleekPredictionPunter.DataInfrastructure.Migrations
{
    public partial class prediction_added_and_remove_fields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Predictions_SportLeagueCategory_CustomCategoryId",
                table: "Predictions");

            migrationBuilder.DropForeignKey(
                name: "FK_Predictions_MatchCategories_MatchCategoryId",
                table: "Predictions");

            migrationBuilder.DropForeignKey(
                name: "FK_Predictions_PricePlans_PricingPlanId",
                table: "Predictions");

            migrationBuilder.DropIndex(
                name: "IX_Predictions_CustomCategoryId",
                table: "Predictions");

            migrationBuilder.DropIndex(
                name: "IX_Predictions_MatchCategoryId",
                table: "Predictions");

            migrationBuilder.DropIndex(
                name: "IX_Predictions_PricingPlanId",
                table: "Predictions");

            migrationBuilder.AddColumn<string>(
                name: "CustomCategory",
                table: "Predictions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatchCategory",
                table: "Predictions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PricingPlanName",
                table: "Predictions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomCategory",
                table: "Predictions");

            migrationBuilder.DropColumn(
                name: "MatchCategory",
                table: "Predictions");

            migrationBuilder.DropColumn(
                name: "PricingPlanName",
                table: "Predictions");

            migrationBuilder.CreateIndex(
                name: "IX_Predictions_CustomCategoryId",
                table: "Predictions",
                column: "CustomCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Predictions_MatchCategoryId",
                table: "Predictions",
                column: "MatchCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Predictions_PricingPlanId",
                table: "Predictions",
                column: "PricingPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Predictions_SportLeagueCategory_CustomCategoryId",
                table: "Predictions",
                column: "CustomCategoryId",
                principalTable: "SportLeagueCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Predictions_MatchCategories_MatchCategoryId",
                table: "Predictions",
                column: "MatchCategoryId",
                principalTable: "MatchCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Predictions_PricePlans_PricingPlanId",
                table: "Predictions",
                column: "PricingPlanId",
                principalTable: "PricePlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
