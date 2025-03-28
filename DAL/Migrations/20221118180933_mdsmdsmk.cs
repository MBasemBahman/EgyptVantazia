﻿using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class mdsmdsmk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<int>(
                name: "Cost",
                table: "AccountSubscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$b1m.Eg2UNnSVjIewRG4a1u5/ABAnEoGWZB/7S7gf2Vya9Jmk98V0y");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "Cost",
                table: "AccountSubscriptions");

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$RvziWwf0JGNRq3NXVDtRZ.fsYILLhDed0wMgp0AjL7tSqMXSnWl3i");
        }
    }
}
