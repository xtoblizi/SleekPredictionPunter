using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SleekPredictionPunter.DataInfrastructure.Migrations
{
    public partial class addedpredictionbookings_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PredictionBookings",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    EntityStatus = table.Column<int>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: true),
                    BookingCodes = table.Column<string>(nullable: true),
                    BookingPlatformIds = table.Column<string>(nullable: true),
                    BookingCodeWithRelationToPlatform = table.Column<string>(nullable: true),
                    Predictions = table.Column<string>(nullable: true),
                    Odd = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    BonusCode = table.Column<string>(nullable: true),
                    PredictedBy = table.Column<string>(nullable: true),
                    LeastMatchstattime = table.Column<DateTime>(nullable: false),
                    PricingPlanId = table.Column<long>(nullable: false),
                    PricingPlan = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PredictionBookings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PredictionBookings");
        }
    }
}
