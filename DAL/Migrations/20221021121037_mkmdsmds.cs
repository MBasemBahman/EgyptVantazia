using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class mkmdsmds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<string>(
                name: "ShirtImageUrl",
                table: "Teams",
                type: "nvarchar(max)",
                nullable: true);

            _ = migrationBuilder.AddColumn<string>(
                name: "ShirtStorageUrl",
                table: "Teams",
                type: "nvarchar(max)",
                nullable: true);

            _ = migrationBuilder.AddColumn<int>(
                name: "FreeTransfer",
                table: "AccountTeams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.AddColumn<bool>(
                name: "IsPrimary",
                table: "AccountTeamPlayerGameWeaks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            _ = migrationBuilder.AddColumn<bool>(
                name: "IsTransfer",
                table: "AccountTeamPlayerGameWeaks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            _ = migrationBuilder.AddColumn<bool>(
                name: "Order",
                table: "AccountTeamPlayerGameWeaks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            _ = migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "AccountTeamPlayerGameWeaks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.AddColumn<bool>(
                name: "DoubleGameWeak",
                table: "AccountTeamGameWeaks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            _ = migrationBuilder.AddColumn<int>(
                name: "TansfarePoints",
                table: "AccountTeamGameWeaks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.AddColumn<bool>(
                name: "Top_11",
                table: "AccountTeamGameWeaks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$E77BCBJjsNMI6vTFxxe/pOIf7xvAxL67r9xFhCEJzyVB7wInMNAWe");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "ShirtImageUrl",
                table: "Teams");

            _ = migrationBuilder.DropColumn(
                name: "ShirtStorageUrl",
                table: "Teams");

            _ = migrationBuilder.DropColumn(
                name: "FreeTransfer",
                table: "AccountTeams");

            _ = migrationBuilder.DropColumn(
                name: "IsPrimary",
                table: "AccountTeamPlayerGameWeaks");

            _ = migrationBuilder.DropColumn(
                name: "IsTransfer",
                table: "AccountTeamPlayerGameWeaks");

            _ = migrationBuilder.DropColumn(
                name: "Order",
                table: "AccountTeamPlayerGameWeaks");

            _ = migrationBuilder.DropColumn(
                name: "Points",
                table: "AccountTeamPlayerGameWeaks");

            _ = migrationBuilder.DropColumn(
                name: "DoubleGameWeak",
                table: "AccountTeamGameWeaks");

            _ = migrationBuilder.DropColumn(
                name: "TansfarePoints",
                table: "AccountTeamGameWeaks");

            _ = migrationBuilder.DropColumn(
                name: "Top_11",
                table: "AccountTeamGameWeaks");

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$f7fKVj7ezshNjpCaTNkZcOVMcEFqR55UjAOS08nikR3t0g1tAjjpa");
        }
    }
}
