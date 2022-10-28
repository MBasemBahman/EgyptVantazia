using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class mksmdmds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<bool>(
                name: "IsCurrent",
                table: "Seasons",
                type: "bit",
                nullable: false,
                defaultValue: false);

            _ = migrationBuilder.AddColumn<int>(
                name: "FinalValue",
                table: "PlayerGameWeakScores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.AddColumn<bool>(
                name: "IsCurrent",
                table: "GameWeaks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$0euBoIpNQYm6umDTbADsnuhkTcqdIQZ5J4xaIEtilC3pipeISJca6");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "IsCurrent",
                table: "Seasons");

            _ = migrationBuilder.DropColumn(
                name: "FinalValue",
                table: "PlayerGameWeakScores");

            _ = migrationBuilder.DropColumn(
                name: "IsCurrent",
                table: "GameWeaks");

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$5ynTA8fyPUTkwU7dX2qiD.IEBNkTZZtaKq1zTCsFnsJ1cZnlJ9FI.");
        }
    }
}
