using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class dkkskdmkds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CountryRankingUpdatedAt",
                table: "AccountTeams",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FavouriteTeamRankingUpdatedAt",
                table: "AccountTeams",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "GlobalRankingUpdatedAt",
                table: "AccountTeams",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$Dvkx4.FOmOYnL6beB.Sm2e00ddD5Dq7Vg2hJ/Hz0zuNW6RDztNFna");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryRankingUpdatedAt",
                table: "AccountTeams");

            migrationBuilder.DropColumn(
                name: "FavouriteTeamRankingUpdatedAt",
                table: "AccountTeams");

            migrationBuilder.DropColumn(
                name: "GlobalRankingUpdatedAt",
                table: "AccountTeams");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$5mY5MYLozd9Q04MJb9yRKOLluMSAreY8aAMYyDUnx254VoAkHPu1C");
        }
    }
}
