using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class Njdnjds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<string>(
            //    name: "Description",
            //    table: "PromoCodes",
            //    type: "nvarchar(max)",
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(max)");

            //migrationBuilder.AlterColumn<string>(
            //    name: "Description",
            //    table: "PromoCodeLang",
            //    type: "nvarchar(max)",
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTo",
                table: "PlayerMarks",
                type: "datetime2",
                nullable: true);

            //migrationBuilder.UpdateData(
            //    table: "Users",
            //    keyColumn: "Id",
            //    keyValue: 1,
            //    column: "Password",
            //    value: "$2a$11$bj/9Algn2xDUOITvqAnNBOQwzXyg5cPIvlnGLwCTEzQiYSRN8d4vi");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTo",
                table: "PlayerMarks");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "PromoCodes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "PromoCodeLang",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$H1H02FtILnaOx//P2t9KHeK5lu/7.Y4XcR2dtjcjmb4C9lZDzesFm");
        }
    }
}
