using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class dsjnjn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "IsDelayed",
                table: "GameWeaks");

            _ = migrationBuilder.AddColumn<bool>(
                name: "IsDelayed",
                table: "TeamGameWeaks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            _ = migrationBuilder.AddColumn<double>(
                name: "CountryRanking",
                table: "AccountTeams",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            _ = migrationBuilder.AddColumn<double>(
                name: "FavouriteTeamRanking",
                table: "AccountTeams",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            _ = migrationBuilder.AddColumn<double>(
                name: "GlobalRanking",
                table: "AccountTeams",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            _ = migrationBuilder.AddColumn<double>(
                name: "CountryRanking",
                table: "AccountTeamGameWeaks",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            _ = migrationBuilder.AddColumn<double>(
                name: "FavouriteTeamRanking",
                table: "AccountTeamGameWeaks",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            _ = migrationBuilder.AddColumn<double>(
                name: "GlobalRanking",
                table: "AccountTeamGameWeaks",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$ut0lrbry5ZDP8p/2erhVFusTERIXUbujOkKoWA.U2lDCIVPK9mXLS");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "IsDelayed",
                table: "TeamGameWeaks");

            _ = migrationBuilder.DropColumn(
                name: "CountryRanking",
                table: "AccountTeams");

            _ = migrationBuilder.DropColumn(
                name: "FavouriteTeamRanking",
                table: "AccountTeams");

            _ = migrationBuilder.DropColumn(
                name: "GlobalRanking",
                table: "AccountTeams");

            _ = migrationBuilder.DropColumn(
                name: "CountryRanking",
                table: "AccountTeamGameWeaks");

            _ = migrationBuilder.DropColumn(
                name: "FavouriteTeamRanking",
                table: "AccountTeamGameWeaks");

            _ = migrationBuilder.DropColumn(
                name: "GlobalRanking",
                table: "AccountTeamGameWeaks");

            _ = migrationBuilder.AddColumn<bool>(
                name: "IsDelayed",
                table: "GameWeaks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$2qEqUijUki.md0hlXSOTpeVytSS9kIYXv9G10S4B/dpR/WqvB65uu");
        }
    }
}
