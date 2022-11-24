using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class kskmskmks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<bool>(
                name: "IsPlayed",
                table: "AccountTeamPlayerGameWeaks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$Xg2LxOZx1pSC3kOnvF9zeeIaXlKeFBeARyPF5wc7R5HEYZCT0/Pmy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "IsPlayed",
                table: "AccountTeamPlayerGameWeaks");

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$ZGBLdVQbPWUNLRq7ZlvZpuBzkt5SEKQKphx7oCap3v5YY/TcEWFZu");
        }
    }
}
