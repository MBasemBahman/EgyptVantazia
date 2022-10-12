using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class afterGameEdit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "_365_MatchUpId",
                table: "TeamGameWeaks");

            _ = migrationBuilder.AddColumn<string>(
                name: "_365_AfterGameStartId",
                table: "Seasons",
                type: "nvarchar(max)",
                nullable: true);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$3aEJ7mN7iIZy94bF7DdtCuNAXCfrkZOcHRNSaqmGUcrnv6yu8Z9Ra");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "_365_AfterGameStartId",
                table: "Seasons");

            _ = migrationBuilder.AddColumn<string>(
                name: "_365_MatchUpId",
                table: "TeamGameWeaks",
                type: "nvarchar(max)",
                nullable: true);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$xEGPuRsFqTuqbCmGjxx2ZuTfleJbmGacfNvpQeXpsFuOEtg5CtS5G");
        }
    }
}
