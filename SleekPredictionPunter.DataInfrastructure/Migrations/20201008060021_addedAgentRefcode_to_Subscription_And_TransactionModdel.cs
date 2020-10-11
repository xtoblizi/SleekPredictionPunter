using Microsoft.EntityFrameworkCore.Migrations;

namespace SleekPredictionPunter.DataInfrastructure.Migrations
{
    public partial class addedAgentRefcode_to_Subscription_And_TransactionModdel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AgentRefCodeForTheSubscriber",
                table: "TransactionLogs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AgentRefCode",
                table: "Subcriptions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgentRefCodeForTheSubscriber",
                table: "TransactionLogs");

            migrationBuilder.DropColumn(
                name: "AgentRefCode",
                table: "Subcriptions");
        }
    }
}
