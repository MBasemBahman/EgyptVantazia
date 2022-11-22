using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class dsdndnds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrippleCaptain",
                table: "AccountTeamPlayerGameWeaks");

            migrationBuilder.AddColumn<int>(
                name: "BenchBoost",
                table: "AccountTeams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DoubleGameWeak",
                table: "AccountTeams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FreeHit",
                table: "AccountTeams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Top_11",
                table: "AccountTeams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TripleCaptain",
                table: "AccountTeams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WildCard",
                table: "AccountTeams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "TripleCaptain",
                table: "AccountTeamGameWeaks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$e9XXqEzNj.Ammb4bPFar9.Ua40bh6ESEPe/o48GDzLZSBNvCAU5Ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BenchBoost",
                table: "AccountTeams");

            migrationBuilder.DropColumn(
                name: "DoubleGameWeak",
                table: "AccountTeams");

            migrationBuilder.DropColumn(
                name: "FreeHit",
                table: "AccountTeams");

            migrationBuilder.DropColumn(
                name: "Top_11",
                table: "AccountTeams");

            migrationBuilder.DropColumn(
                name: "TripleCaptain",
                table: "AccountTeams");

            migrationBuilder.DropColumn(
                name: "WildCard",
                table: "AccountTeams");

            migrationBuilder.DropColumn(
                name: "TripleCaptain",
                table: "AccountTeamGameWeaks");

            migrationBuilder.AddColumn<bool>(
                name: "TrippleCaptain",
                table: "AccountTeamPlayerGameWeaks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$SqOqPCWiBnIKmiNbqEq5yO6zw0I.gSfa5L9ZzXKNT0Li5LZKCkFq6");
        }
    }
}
