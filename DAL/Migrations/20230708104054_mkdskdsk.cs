using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class mkdskdsk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Fk_FavouriteTeam",
                table: "AccountTeams",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$M4FIK3eXjrKkUbPHoDDZk.Ruf0Cdg1EETC9DnLp0iYRma8P/BAj.S");

            migrationBuilder.CreateIndex(
                name: "IX_AccountTeams_Fk_FavouriteTeam",
                table: "AccountTeams",
                column: "Fk_FavouriteTeam");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountTeams_Teams_Fk_FavouriteTeam",
                table: "AccountTeams",
                column: "Fk_FavouriteTeam",
                principalTable: "Teams",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountTeams_Teams_Fk_FavouriteTeam",
                table: "AccountTeams");

            migrationBuilder.DropIndex(
                name: "IX_AccountTeams_Fk_FavouriteTeam",
                table: "AccountTeams");

            migrationBuilder.DropColumn(
                name: "Fk_FavouriteTeam",
                table: "AccountTeams");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$XKe4nOkvKeLz4FcxnerzJOAgIg607eqD2.AfnmIxfy9hQIAkxkQ6K");
        }
    }
}
