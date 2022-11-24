using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class dsmdsmkm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "AlphaName",
                table: "Countries");

            _ = migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Countries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$ssftLF36dtXINI7yYsSOweS2NwGRwzMF/IofgYfG2yheLUUNgVo5O");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "Order",
                table: "Countries");

            _ = migrationBuilder.AddColumn<string>(
                name: "AlphaName",
                table: "Countries",
                type: "nvarchar(max)",
                nullable: true);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$VIi3EkFB674ZC1GU.dJanOETO7H9Kjd6u7znYHPl324oFevlYYVD2");
        }
    }
}
