using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class GameWeekValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "_365_GameWeakIdValue",
                table: "GameWeaks",
                type: "int",
                nullable: false,
                computedColumnSql: "CONVERT(int, SUBSTRING(_365_GameWeakId, 2, LEN(_365_GameWeakId)))",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$vdzkUUjvLPLc7T1mg59oXub5dAyNGTIOAa82xBXHglTcrFrev9ySi");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "_365_GameWeakIdValue",
                table: "GameWeaks",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComputedColumnSql: "CONVERT(int, SUBSTRING(_365_GameWeakId, 2, LEN(_365_GameWeakId)))");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$g1xeTuUF7EGo.M36MLbPCeb8YuouLuLNkipbIketJwE5cp3mP9NkG");
        }
    }
}
