using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class mkmdkmdm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<double>(
                name: "Ranking",
                table: "PrivateLeagueMembers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            _ = migrationBuilder.AddColumn<bool>(
                name: "IsDelayed",
                table: "GameWeaks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$2qEqUijUki.md0hlXSOTpeVytSS9kIYXv9G10S4B/dpR/WqvB65uu");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "Ranking",
                table: "PrivateLeagueMembers");

            _ = migrationBuilder.DropColumn(
                name: "IsDelayed",
                table: "GameWeaks");

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$E77BCBJjsNMI6vTFxxe/pOIf7xvAxL67r9xFhCEJzyVB7wInMNAWe");
        }
    }
}
