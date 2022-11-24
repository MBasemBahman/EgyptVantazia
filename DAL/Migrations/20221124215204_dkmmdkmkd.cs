﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class dkmmdkmkd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "SeasonGlobalRanking",
                table: "AccountTeamGameWeaks",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "SeasonTotalPoints",
                table: "AccountTeamGameWeaks",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$HafWZiiZ17Ou4Ge00iWWIuvDJdIM52TmM4J9CDUxvB98I.Z0E74Ta");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeasonGlobalRanking",
                table: "AccountTeamGameWeaks");

            migrationBuilder.DropColumn(
                name: "SeasonTotalPoints",
                table: "AccountTeamGameWeaks");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$dSs2QAxfBoE75IgGv1qbke.e/5b9uB0uwvMbE5G0aN/DETBYavXVK");
        }
    }
}
