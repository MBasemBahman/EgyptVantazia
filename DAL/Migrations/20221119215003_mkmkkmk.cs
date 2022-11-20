using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class mkmkkmk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsPlayed",
                table: "AccountTeamPlayerGameWeaks",
                newName: "HavePointsInTotal");

            migrationBuilder.AddColumn<bool>(
                name: "HavePoints",
                table: "AccountTeamPlayerGameWeaks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$SqOqPCWiBnIKmiNbqEq5yO6zw0I.gSfa5L9ZzXKNT0Li5LZKCkFq6");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HavePoints",
                table: "AccountTeamPlayerGameWeaks");

            migrationBuilder.RenameColumn(
                name: "HavePointsInTotal",
                table: "AccountTeamPlayerGameWeaks",
                newName: "IsPlayed");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$Xg2LxOZx1pSC3kOnvF9zeeIaXlKeFBeARyPF5wc7R5HEYZCT0/Pmy");
        }
    }
}
