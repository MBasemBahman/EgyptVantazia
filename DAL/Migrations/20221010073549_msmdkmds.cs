﻿using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class msmdkmds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<bool>(
                name: "IsEnded",
                table: "TeamGameWeaks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$.BAbheoXcd92wJSio8Nt0.4sK3GMH6aGk5H4VM9DXd5eN4pw9Rgwe");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "IsEnded",
                table: "TeamGameWeaks");

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$rJE24T6rOfSJdhAVxoAvdO9NldxjvjAC9VMDjvWQGxkbAlRTeR0Li");
        }
    }
}
