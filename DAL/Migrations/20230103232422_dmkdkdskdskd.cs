using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class dmkdkdskdskd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCanNotEdit",
                table: "ScoreTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Fk_GameWeak",
                table: "PrivateLeagues",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "PrivateLeagueMembers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanNotEdit",
                table: "PlayerGameWeakScores",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$tQnHJJLj3HYRBAvl37CD3u3d.NuAEi11qTXdDr8TZgSPi1GubU8NS");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateLeagues_Fk_GameWeak",
                table: "PrivateLeagues",
                column: "Fk_GameWeak");

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateLeagues_GameWeaks_Fk_GameWeak",
                table: "PrivateLeagues",
                column: "Fk_GameWeak",
                principalTable: "GameWeaks",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrivateLeagues_GameWeaks_Fk_GameWeak",
                table: "PrivateLeagues");

            migrationBuilder.DropIndex(
                name: "IX_PrivateLeagues_Fk_GameWeak",
                table: "PrivateLeagues");

            migrationBuilder.DropColumn(
                name: "IsCanNotEdit",
                table: "ScoreTypes");

            migrationBuilder.DropColumn(
                name: "Fk_GameWeak",
                table: "PrivateLeagues");

            migrationBuilder.DropColumn(
                name: "Points",
                table: "PrivateLeagueMembers");

            migrationBuilder.DropColumn(
                name: "IsCanNotEdit",
                table: "PlayerGameWeakScores");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$kag6HmHkEoj2q4dwpD8Ma.w2tF7/3VvmduKbYzwmxz4RCRhdO5bx2");
        }
    }
}
