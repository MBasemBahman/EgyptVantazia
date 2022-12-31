using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class mdmfkmdm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SecondJobId",
                table: "TeamGameWeaks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThirdJobId",
                table: "TeamGameWeaks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "GameWeaks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EndTimeJobId",
                table: "GameWeaks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$kag6HmHkEoj2q4dwpD8Ma.w2tF7/3VvmduKbYzwmxz4RCRhdO5bx2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecondJobId",
                table: "TeamGameWeaks");

            migrationBuilder.DropColumn(
                name: "ThirdJobId",
                table: "TeamGameWeaks");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "GameWeaks");

            migrationBuilder.DropColumn(
                name: "EndTimeJobId",
                table: "GameWeaks");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$D9e.o3DrrzJ/IM9PI.ebM.6uQ0urTMRbpYMw4ITaAAIhKCOSY7X/i");
        }
    }
}
