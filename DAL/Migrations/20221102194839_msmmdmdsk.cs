using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class msmmdmdsk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "PositionByPercent",
                table: "PlayerSeasonScoreStates");

            _ = migrationBuilder.DropColumn(
                name: "PositionByPoints",
                table: "PlayerSeasonScoreStates");

            _ = migrationBuilder.DropColumn(
                name: "PositionByValue",
                table: "PlayerSeasonScoreStates");

            _ = migrationBuilder.DropColumn(
                name: "PositionByPercent",
                table: "PlayerGameWeakScoreStates");

            _ = migrationBuilder.DropColumn(
                name: "PositionByPoints",
                table: "PlayerGameWeakScoreStates");

            _ = migrationBuilder.DropColumn(
                name: "PositionByValue",
                table: "PlayerGameWeakScoreStates");

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$2CTWHkJ1lAv9Q2iT1EZAo.PHe7pT.bDbdSZGOQ2sJx975zbAJpcea");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<int>(
                name: "PositionByPercent",
                table: "PlayerSeasonScoreStates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.AddColumn<int>(
                name: "PositionByPoints",
                table: "PlayerSeasonScoreStates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.AddColumn<int>(
                name: "PositionByValue",
                table: "PlayerSeasonScoreStates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.AddColumn<int>(
                name: "PositionByPercent",
                table: "PlayerGameWeakScoreStates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.AddColumn<int>(
                name: "PositionByPoints",
                table: "PlayerGameWeakScoreStates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.AddColumn<int>(
                name: "PositionByValue",
                table: "PlayerGameWeakScoreStates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$NTEuwaxMiEQTk5aho2yNn.kh5V.wb.gv00LHryvtqysB0PqRM0yPa");
        }
    }
}
