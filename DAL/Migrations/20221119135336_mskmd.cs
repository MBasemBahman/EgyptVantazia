using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class mskmd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Top15",
                table: "PlayerSeasonScoreStates",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Top15",
                table: "PlayerGameWeakScoreStates",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$ZGBLdVQbPWUNLRq7ZlvZpuBzkt5SEKQKphx7oCap3v5YY/TcEWFZu");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Top15",
                table: "PlayerSeasonScoreStates");

            migrationBuilder.DropColumn(
                name: "Top15",
                table: "PlayerGameWeakScoreStates");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$b1m.Eg2UNnSVjIewRG4a1u5/ABAnEoGWZB/7S7gf2Vya9Jmk98V0y");
        }
    }
}
