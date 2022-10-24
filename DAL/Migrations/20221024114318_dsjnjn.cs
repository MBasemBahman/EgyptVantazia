using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class dsjnjn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelayed",
                table: "GameWeaks");

            migrationBuilder.AddColumn<bool>(
                name: "IsDelayed",
                table: "TeamGameWeaks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "CountryRanking",
                table: "AccountTeams",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "FavouriteTeamRanking",
                table: "AccountTeams",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "GlobalRanking",
                table: "AccountTeams",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CountryRanking",
                table: "AccountTeamGameWeaks",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "FavouriteTeamRanking",
                table: "AccountTeamGameWeaks",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "GlobalRanking",
                table: "AccountTeamGameWeaks",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$ut0lrbry5ZDP8p/2erhVFusTERIXUbujOkKoWA.U2lDCIVPK9mXLS");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelayed",
                table: "TeamGameWeaks");

            migrationBuilder.DropColumn(
                name: "CountryRanking",
                table: "AccountTeams");

            migrationBuilder.DropColumn(
                name: "FavouriteTeamRanking",
                table: "AccountTeams");

            migrationBuilder.DropColumn(
                name: "GlobalRanking",
                table: "AccountTeams");

            migrationBuilder.DropColumn(
                name: "CountryRanking",
                table: "AccountTeamGameWeaks");

            migrationBuilder.DropColumn(
                name: "FavouriteTeamRanking",
                table: "AccountTeamGameWeaks");

            migrationBuilder.DropColumn(
                name: "GlobalRanking",
                table: "AccountTeamGameWeaks");

            migrationBuilder.AddColumn<bool>(
                name: "IsDelayed",
                table: "GameWeaks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$2qEqUijUki.md0hlXSOTpeVytSS9kIYXv9G10S4B/dpR/WqvB65uu");
        }
    }
}
