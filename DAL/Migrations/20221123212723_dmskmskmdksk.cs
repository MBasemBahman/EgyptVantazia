using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class dmskmskmdksk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "_365_GameWeakIdValue",
                table: "GameWeaks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$7nc/0vdvENqzTa1qjXfCIeHdGLLLjL/9fKwYn.rSUmj91rGpL.64O");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "_365_GameWeakIdValue",
                table: "GameWeaks");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$e9XXqEzNj.Ammb4bPFar9.Ua40bh6ESEPe/o48GDzLZSBNvCAU5Ci");
        }
    }
}
