using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class TwiceCpatian : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TwiceCaptain",
                table: "AccountTeams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "TwiceCaptain",
                table: "AccountTeamGameWeaks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$J7HiQ/lxFwX6.P.VtD8oPeZn473L0yeQqE1wA5SMol2h9.7C31eAO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TwiceCaptain",
                table: "AccountTeams");

            migrationBuilder.DropColumn(
                name: "TwiceCaptain",
                table: "AccountTeamGameWeaks");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$C03Yv8OFRkXHcOboxAVJCOOzEyXwq1ioXTq9pjM2UvnGOHxcoNOyK");
        }
    }
}
