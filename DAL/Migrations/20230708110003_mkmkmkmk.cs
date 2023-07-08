using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class mkmkmkmk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountTeams_Teams_Fk_FavouriteTeam",
                table: "AccountTeams");

            migrationBuilder.AlterColumn<int>(
                name: "Fk_FavouriteTeam",
                table: "AccountTeams",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$IUgd3krE2KE3TvEzbXQn9OoPQ5P9K62/.meCF.BioZ562BDofIEy2");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountTeams_Teams_Fk_FavouriteTeam",
                table: "AccountTeams",
                column: "Fk_FavouriteTeam",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountTeams_Teams_Fk_FavouriteTeam",
                table: "AccountTeams");

            migrationBuilder.AlterColumn<int>(
                name: "Fk_FavouriteTeam",
                table: "AccountTeams",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$M4FIK3eXjrKkUbPHoDDZk.Ruf0Cdg1EETC9DnLp0iYRma8P/BAj.S");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountTeams_Teams_Fk_FavouriteTeam",
                table: "AccountTeams",
                column: "Fk_FavouriteTeam",
                principalTable: "Teams",
                principalColumn: "Id");
        }
    }
}
