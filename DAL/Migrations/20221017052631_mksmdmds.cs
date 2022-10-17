using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class mksmdmds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCurrent",
                table: "Seasons",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "FinalValue",
                table: "PlayerGameWeakScores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsCurrent",
                table: "GameWeaks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$0euBoIpNQYm6umDTbADsnuhkTcqdIQZ5J4xaIEtilC3pipeISJca6");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCurrent",
                table: "Seasons");

            migrationBuilder.DropColumn(
                name: "FinalValue",
                table: "PlayerGameWeakScores");

            migrationBuilder.DropColumn(
                name: "IsCurrent",
                table: "GameWeaks");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$5ynTA8fyPUTkwU7dX2qiD.IEBNkTZZtaKq1zTCsFnsJ1cZnlJ9FI.");
        }
    }
}
