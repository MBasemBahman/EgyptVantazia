using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class mkdkmsk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AlterColumn<double>(
                name: "Cost",
                table: "PlayerTransfers",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            _ = migrationBuilder.AlterColumn<double>(
                name: "TotalMoney",
                table: "AccountTeams",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$kKvISOQ7puPwJSw1xbGkkeWKboy9QFRm5JHdFNvqeJ/8oegEHWDPm");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AlterColumn<int>(
                name: "Cost",
                table: "PlayerTransfers",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            _ = migrationBuilder.AlterColumn<int>(
                name: "TotalMoney",
                table: "AccountTeams",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$vNwgx5LJin4b1jtJXfz6ROwgh5EGlm0NWgYm6RFAqojMiipM1o9mK");
        }
    }
}
