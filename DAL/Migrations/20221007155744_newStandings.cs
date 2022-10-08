using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class newStandings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "_365_For",
                table: "Standings",
                newName: "Strike");

            migrationBuilder.RenameColumn(
                name: "Points",
                table: "Standings",
                newName: "Position");

            migrationBuilder.AlterColumn<double>(
                name: "Ratio",
                table: "Standings",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "For",
                table: "Standings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$xEGPuRsFqTuqbCmGjxx2ZuTfleJbmGacfNvpQeXpsFuOEtg5CtS5G");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "For",
                table: "Standings");

            migrationBuilder.RenameColumn(
                name: "Strike",
                table: "Standings",
                newName: "_365_For");

            migrationBuilder.RenameColumn(
                name: "Position",
                table: "Standings",
                newName: "Points");

            migrationBuilder.AlterColumn<int>(
                name: "Ratio",
                table: "Standings",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$FfIkZkJSQLK552mTQhlDsuqA1MKVL8j9GNVRljjyc1Fza/nIVQw4C");
        }
    }
}
