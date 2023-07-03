using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class msmds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "GoldSubscriptionRanking",
                table: "AccountTeams",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "GoldSubscriptionUpdatedAt",
                table: "AccountTeams",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "UnSubscriptionRanking",
                table: "AccountTeams",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UnSubscriptionUpdatedAt",
                table: "AccountTeams",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SeasonGoldSubscriptionRanking",
                table: "AccountTeamGameWeaks",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SeasonUnSubscriptionRanking",
                table: "AccountTeamGameWeaks",
                type: "float",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$XKe4nOkvKeLz4FcxnerzJOAgIg607eqD2.AfnmIxfy9hQIAkxkQ6K");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoldSubscriptionRanking",
                table: "AccountTeams");

            migrationBuilder.DropColumn(
                name: "GoldSubscriptionUpdatedAt",
                table: "AccountTeams");

            migrationBuilder.DropColumn(
                name: "UnSubscriptionRanking",
                table: "AccountTeams");

            migrationBuilder.DropColumn(
                name: "UnSubscriptionUpdatedAt",
                table: "AccountTeams");

            migrationBuilder.DropColumn(
                name: "SeasonGoldSubscriptionRanking",
                table: "AccountTeamGameWeaks");

            migrationBuilder.DropColumn(
                name: "SeasonUnSubscriptionRanking",
                table: "AccountTeamGameWeaks");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$aRAD7exKm6eg4h.ztdYdE.pb0WYxUEgr1oL7Jn7oV/BlU2Ru6iF36");
        }
    }
}
