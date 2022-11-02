using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class msmmdmdsk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PositionByPercent",
                table: "PlayerSeasonScoreStates");

            migrationBuilder.DropColumn(
                name: "PositionByPoints",
                table: "PlayerSeasonScoreStates");

            migrationBuilder.DropColumn(
                name: "PositionByValue",
                table: "PlayerSeasonScoreStates");

            migrationBuilder.DropColumn(
                name: "PositionByPercent",
                table: "PlayerGameWeakScoreStates");

            migrationBuilder.DropColumn(
                name: "PositionByPoints",
                table: "PlayerGameWeakScoreStates");

            migrationBuilder.DropColumn(
                name: "PositionByValue",
                table: "PlayerGameWeakScoreStates");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$2CTWHkJ1lAv9Q2iT1EZAo.PHe7pT.bDbdSZGOQ2sJx975zbAJpcea");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PositionByPercent",
                table: "PlayerSeasonScoreStates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PositionByPoints",
                table: "PlayerSeasonScoreStates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PositionByValue",
                table: "PlayerSeasonScoreStates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PositionByPercent",
                table: "PlayerGameWeakScoreStates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PositionByPoints",
                table: "PlayerGameWeakScoreStates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PositionByValue",
                table: "PlayerGameWeakScoreStates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$NTEuwaxMiEQTk5aho2yNn.kh5V.wb.gv00LHryvtqysB0PqRM0yPa");
        }
    }
}
