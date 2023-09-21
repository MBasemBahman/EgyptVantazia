using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class DSdsmdskmds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "_365_GameWeakIdValue",
                table: "GameWeaks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldComputedColumnSql: "CONVERT(int, SUBSTRING(_365_GameWeakId, 2, LEN(_365_GameWeakId)))");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$JeHC1yrk4pS9rwteacwSV.M.JfHM7YkUgfZnG4JVqSI/4ZEEsBBku");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "_365_GameWeakIdValue",
                table: "GameWeaks",
                type: "int",
                nullable: false,
                computedColumnSql: "CONVERT(int, SUBSTRING(_365_GameWeakId, 2, LEN(_365_GameWeakId)))",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$vdzkUUjvLPLc7T1mg59oXub5dAyNGTIOAa82xBXHglTcrFrev9ySi");
        }
    }
}
