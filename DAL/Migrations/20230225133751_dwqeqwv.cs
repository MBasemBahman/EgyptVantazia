using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class dwqeqwv : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCanNotEdit",
                table: "PlayerGameWeaks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$TYy52fuZ/MLDhP/4AaFRgeJbsbx6RTabczmEvev02lgv2Hmca3A/G");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCanNotEdit",
                table: "PlayerGameWeaks");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$zD/Y1zvD48JyflKd/VkRvOK7FdQZ1MzhlmNaqMU7oVdO8bBtWUQHO");
        }
    }
}
