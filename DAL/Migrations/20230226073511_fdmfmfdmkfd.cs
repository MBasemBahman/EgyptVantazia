using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class fdmfmfdmkfd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$VIUhHbBUESDgHLJ/sATPie9SFaDuQd6cGz8KRrPHYwLYCxwJVAhLS");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$TYy52fuZ/MLDhP/4AaFRgeJbsbx6RTabczmEvev02lgv2Hmca3A/G");
        }
    }
}
