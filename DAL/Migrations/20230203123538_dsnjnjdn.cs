using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class dsnjnjdn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CountryRankingUpdatedAt",
                table: "AccountTeamGameWeaks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FavouriteTeamRankingUpdatedAt",
                table: "AccountTeamGameWeaks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "GlobalRankingUpdatedAt",
                table: "AccountTeamGameWeaks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$zD/Y1zvD48JyflKd/VkRvOK7FdQZ1MzhlmNaqMU7oVdO8bBtWUQHO");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryRankingUpdatedAt",
                table: "AccountTeamGameWeaks");

            migrationBuilder.DropColumn(
                name: "FavouriteTeamRankingUpdatedAt",
                table: "AccountTeamGameWeaks");

            migrationBuilder.DropColumn(
                name: "GlobalRankingUpdatedAt",
                table: "AccountTeamGameWeaks");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$Dvkx4.FOmOYnL6beB.Sm2e00ddD5Dq7Vg2hJ/Hz0zuNW6RDztNFna");
        }
    }
}
