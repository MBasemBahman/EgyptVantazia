using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class dsdmdsmdm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<int>(
                name: "TotalPoints",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.AddColumn<int>(
                name: "TotalPoints",
                table: "PlayerGameWeaks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$.DA.ZQhCqnt2omXHU/noX.Af0XRVOLHdyr2uU4ZMSFyWxTl.hoMfW");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "TotalPoints",
                table: "Players");

            _ = migrationBuilder.DropColumn(
                name: "TotalPoints",
                table: "PlayerGameWeaks");

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$ut0lrbry5ZDP8p/2erhVFusTERIXUbujOkKoWA.U2lDCIVPK9mXLS");
        }
    }
}
