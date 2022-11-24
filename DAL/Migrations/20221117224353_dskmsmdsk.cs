using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class dskmsmdsk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<bool>(
                name: "IsNext",
                table: "GameWeaks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            _ = migrationBuilder.AddColumn<bool>(
                name: "IsPrev",
                table: "GameWeaks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$TLG.cB6Zfxd3QR3hNlBGkuiOPMICgvdN.O8xLvFH1vbfZoNAtaP8S");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "IsNext",
                table: "GameWeaks");

            _ = migrationBuilder.DropColumn(
                name: "IsPrev",
                table: "GameWeaks");

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$VcNslDXA/gr9ZuFHXIT2aewi/CxXZWc6u2UtEnH3STYxadEiqpw5u");
        }
    }
}
