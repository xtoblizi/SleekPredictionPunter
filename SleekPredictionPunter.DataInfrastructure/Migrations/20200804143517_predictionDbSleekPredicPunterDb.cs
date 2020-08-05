using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SleekPredictionPunter.DataInfrastructure.Migrations
{
    public partial class predictionDbSleekPredicPunterDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PredictorUsers",
                table: "PredictorUsers");

            migrationBuilder.RenameTable(
                name: "PredictorUsers",
                newName: "Predictor");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Predictor",
                table: "Predictor",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Predictions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    EntityStatus = table.Column<int>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: true),
                    PredictorUserName = table.Column<string>(nullable: true),
                    ClubA = table.Column<string>(nullable: true),
                    ClubALogoPath = table.Column<string>(nullable: true),
                    ClubB = table.Column<string>(nullable: true),
                    ClubBLogoPath = table.Column<string>(nullable: true),
                    PredictionValue = table.Column<string>(nullable: true),
                    TimeofFixture = table.Column<DateTime>(nullable: false),
                    SubscriberId = table.Column<long>(nullable: true),
                    PredictorId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Predictions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Predictions_Predictor_PredictorId",
                        column: x => x.PredictorId,
                        principalTable: "Predictor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Predictions_Subscribers_SubscriberId",
                        column: x => x.SubscriberId,
                        principalTable: "Subscribers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Predictions_PredictorId",
                table: "Predictions",
                column: "PredictorId");

            migrationBuilder.CreateIndex(
                name: "IX_Predictions_SubscriberId",
                table: "Predictions",
                column: "SubscriberId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Predictions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Predictor",
                table: "Predictor");

            migrationBuilder.RenameTable(
                name: "Predictor",
                newName: "PredictorUsers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PredictorUsers",
                table: "PredictorUsers",
                column: "Id");
        }
    }
}
