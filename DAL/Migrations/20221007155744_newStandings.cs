using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class newStandings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.RenameColumn(
                name: "_365_For",
                table: "Standings",
                newName: "Strike");

            _ = migrationBuilder.RenameColumn(
                name: "Points",
                table: "Standings",
                newName: "Position");

            _ = migrationBuilder.AlterColumn<double>(
                name: "Ratio",
                table: "Standings",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            _ = migrationBuilder.AddColumn<int>(
                name: "For",
                table: "Standings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$xEGPuRsFqTuqbCmGjxx2ZuTfleJbmGacfNvpQeXpsFuOEtg5CtS5G");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "For",
                table: "Standings");

            _ = migrationBuilder.RenameColumn(
                name: "Strike",
                table: "Standings",
                newName: "_365_For");

            _ = migrationBuilder.RenameColumn(
                name: "Position",
                table: "Standings",
                newName: "Points");

            _ = migrationBuilder.AlterColumn<int>(
                name: "Ratio",
                table: "Standings",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$FfIkZkJSQLK552mTQhlDsuqA1MKVL8j9GNVRljjyc1Fza/nIVQw4C");
        }
    }
}
