using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class mkmkmk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<int>(
                name: "PrevPoints",
                table: "AccountTeamGameWeaks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$RvziWwf0JGNRq3NXVDtRZ.fsYILLhDed0wMgp0AjL7tSqMXSnWl3i");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "PrevPoints",
                table: "AccountTeamGameWeaks");

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$TLG.cB6Zfxd3QR3hNlBGkuiOPMICgvdN.O8xLvFH1vbfZoNAtaP8S");
        }
    }
}
