using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class afterGameEdit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "_365_MatchUpId",
                table: "TeamGameWeaks");

            migrationBuilder.AddColumn<string>(
                name: "_365_AfterGameStartId",
                table: "Seasons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$3aEJ7mN7iIZy94bF7DdtCuNAXCfrkZOcHRNSaqmGUcrnv6yu8Z9Ra");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "_365_AfterGameStartId",
                table: "Seasons");

            migrationBuilder.AddColumn<string>(
                name: "_365_MatchUpId",
                table: "TeamGameWeaks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$xEGPuRsFqTuqbCmGjxx2ZuTfleJbmGacfNvpQeXpsFuOEtg5CtS5G");
        }
    }
}
