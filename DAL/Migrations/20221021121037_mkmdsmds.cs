using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class mkmdsmds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShirtImageUrl",
                table: "Teams",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShirtStorageUrl",
                table: "Teams",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FreeTransfer",
                table: "AccountTeams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrimary",
                table: "AccountTeamPlayerGameWeaks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTransfer",
                table: "AccountTeamPlayerGameWeaks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Order",
                table: "AccountTeamPlayerGameWeaks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "AccountTeamPlayerGameWeaks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "DoubleGameWeak",
                table: "AccountTeamGameWeaks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TansfarePoints",
                table: "AccountTeamGameWeaks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Top_11",
                table: "AccountTeamGameWeaks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$E77BCBJjsNMI6vTFxxe/pOIf7xvAxL67r9xFhCEJzyVB7wInMNAWe");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShirtImageUrl",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "ShirtStorageUrl",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "FreeTransfer",
                table: "AccountTeams");

            migrationBuilder.DropColumn(
                name: "IsPrimary",
                table: "AccountTeamPlayerGameWeaks");

            migrationBuilder.DropColumn(
                name: "IsTransfer",
                table: "AccountTeamPlayerGameWeaks");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "AccountTeamPlayerGameWeaks");

            migrationBuilder.DropColumn(
                name: "Points",
                table: "AccountTeamPlayerGameWeaks");

            migrationBuilder.DropColumn(
                name: "DoubleGameWeak",
                table: "AccountTeamGameWeaks");

            migrationBuilder.DropColumn(
                name: "TansfarePoints",
                table: "AccountTeamGameWeaks");

            migrationBuilder.DropColumn(
                name: "Top_11",
                table: "AccountTeamGameWeaks");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$f7fKVj7ezshNjpCaTNkZcOVMcEFqR55UjAOS08nikR3t0g1tAjjpa");
        }
    }
}
