using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class Njds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Teams_Fk_FavouriteTeam",
                table: "Accounts");

            migrationBuilder.AlterColumn<int>(
                name: "Fk_FavouriteTeam",
                table: "Accounts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$uIrdHe9OZAVIbh.IMLXn2uWZIjJXCQKUyT3gKnwQgwVfdZ7p74yoe");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Teams_Fk_FavouriteTeam",
                table: "Accounts",
                column: "Fk_FavouriteTeam",
                principalTable: "Teams",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Teams_Fk_FavouriteTeam",
                table: "Accounts");

            migrationBuilder.AlterColumn<int>(
                name: "Fk_FavouriteTeam",
                table: "Accounts",
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
                name: "FK_Accounts_Teams_Fk_FavouriteTeam",
                table: "Accounts",
                column: "Fk_FavouriteTeam",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
