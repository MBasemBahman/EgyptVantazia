using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class dmskmskmdksk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<int>(
                name: "_365_GameWeakIdValue",
                table: "GameWeaks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$7nc/0vdvENqzTa1qjXfCIeHdGLLLjL/9fKwYn.rSUmj91rGpL.64O");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "_365_GameWeakIdValue",
                table: "GameWeaks");

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$e9XXqEzNj.Ammb4bPFar9.Ua40bh6ESEPe/o48GDzLZSBNvCAU5Ci");
        }
    }
}
