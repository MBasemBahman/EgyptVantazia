﻿using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class njsnjnj : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<bool>(
                name: "IsVip",
                table: "AccountTeams",
                type: "bit",
                nullable: false,
                defaultValue: false);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$vNwgx5LJin4b1jtJXfz6ROwgh5EGlm0NWgYm6RFAqojMiipM1o9mK");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "IsVip",
                table: "AccountTeams");

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$HafWZiiZ17Ou4Ge00iWWIuvDJdIM52TmM4J9CDUxvB98I.Z0E74Ta");
        }
    }
}
