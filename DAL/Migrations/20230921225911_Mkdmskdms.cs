using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class Mkdmskdms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "_365_GameWeakIdValue",
                table: "GameWeaks",
                type: "int",
                nullable: false,
                computedColumnSql: "CONVERT(int, _365_GameWeakId)",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$tWYCYPvy2WMIggFi8aWxMOwDQbpF.Ahhf4MHq.QUvV7CSol1QDds6");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "_365_GameWeakIdValue",
                table: "GameWeaks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldComputedColumnSql: "CONVERT(int, _365_GameWeakId)");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$JeHC1yrk4pS9rwteacwSV.M.JfHM7YkUgfZnG4JVqSI/4ZEEsBBku");
        }
    }
}
