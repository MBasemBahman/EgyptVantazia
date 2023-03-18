using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class dkmdksmks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ShowAt",
                table: "Notifications");

            migrationBuilder.AddColumn<bool>(
                name: "ShowAds",
                table: "Accounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$4pWep1S1mCbwUZh8CWEJPOrKX7X5lnyX7TncSRmzekrtFfi6p1RjG");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowAds",
                table: "Accounts");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "Notifications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ShowAt",
                table: "Notifications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$319o9zp.mamiWdk1X9IbBO0i1QqrZiyfh1VAK2iq345EOcpHYW33m");
        }
    }
}
