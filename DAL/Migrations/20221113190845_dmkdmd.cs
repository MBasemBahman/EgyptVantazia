﻿using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class dmkdmd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AccountSubscriptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$VdTgR8JNAcsoKH74/oELJe4KKY9kY3uAb1JbZMayLLNDlR.1EMAIi");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AccountSubscriptions");

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$q7VdwMZCq5qM5KSaioaelOvO95xJV7CdYP0al7zyUyXHFw3PvJKpW");
        }
    }
}
