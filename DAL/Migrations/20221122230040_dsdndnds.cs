using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class dsdndnds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "TrippleCaptain",
                table: "AccountTeamPlayerGameWeaks");

            _ = migrationBuilder.AddColumn<int>(
                name: "BenchBoost",
                table: "AccountTeams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.AddColumn<int>(
                name: "DoubleGameWeak",
                table: "AccountTeams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.AddColumn<int>(
                name: "FreeHit",
                table: "AccountTeams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.AddColumn<int>(
                name: "Top_11",
                table: "AccountTeams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.AddColumn<int>(
                name: "TripleCaptain",
                table: "AccountTeams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.AddColumn<int>(
                name: "WildCard",
                table: "AccountTeams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.AddColumn<bool>(
                name: "TripleCaptain",
                table: "AccountTeamGameWeaks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$e9XXqEzNj.Ammb4bPFar9.Ua40bh6ESEPe/o48GDzLZSBNvCAU5Ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "BenchBoost",
                table: "AccountTeams");

            _ = migrationBuilder.DropColumn(
                name: "DoubleGameWeak",
                table: "AccountTeams");

            _ = migrationBuilder.DropColumn(
                name: "FreeHit",
                table: "AccountTeams");

            _ = migrationBuilder.DropColumn(
                name: "Top_11",
                table: "AccountTeams");

            _ = migrationBuilder.DropColumn(
                name: "TripleCaptain",
                table: "AccountTeams");

            _ = migrationBuilder.DropColumn(
                name: "WildCard",
                table: "AccountTeams");

            _ = migrationBuilder.DropColumn(
                name: "TripleCaptain",
                table: "AccountTeamGameWeaks");

            _ = migrationBuilder.AddColumn<bool>(
                name: "TrippleCaptain",
                table: "AccountTeamPlayerGameWeaks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$SqOqPCWiBnIKmiNbqEq5yO6zw0I.gSfa5L9ZzXKNT0Li5LZKCkFq6");
        }
    }
}
