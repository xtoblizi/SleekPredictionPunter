using Microsoft.EntityFrameworkCore.Migrations;

namespace SleekPredictionPunter.DataInfrastructure.Migrations
{
    public partial class walletupdateonfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmountCredited",
                table: "WalletModel",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmountDebited",
                table: "WalletModel",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalAmountCredited",
                table: "WalletModel");

            migrationBuilder.DropColumn(
                name: "TotalAmountDebited",
                table: "WalletModel");
        }
    }
}
