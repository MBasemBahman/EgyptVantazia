using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class msmsmdsms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "PlayerGameWeaks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$pPlGGFPyy9KqUaWCqQOegeBprqBJDKTDylXkyaNMMNLdg7Nd1t8qm");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "Position",
                table: "Players");

            _ = migrationBuilder.DropColumn(
                name: "Position",
                table: "PlayerGameWeaks");

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$.DA.ZQhCqnt2omXHU/noX.Af0XRVOLHdyr2uU4ZMSFyWxTl.hoMfW");
        }
    }
}
