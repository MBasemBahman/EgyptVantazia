﻿using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class msmkmdm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "PlayerGameWeakScores",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$rylYIcKhXwliYYb6.1/4guRTZBfSIPztuQvHuFhUUPHaYhC.e2tM2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AlterColumn<int>(
                name: "Value",
                table: "PlayerGameWeakScores",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$YTFTNq6TriyL8KQ9BnZmwO45ylGBcud5RBRuVFs7q7HfTd0DCg7Cu");
        }
    }
}
