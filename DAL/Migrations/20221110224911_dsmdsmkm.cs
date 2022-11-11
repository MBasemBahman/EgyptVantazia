using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class dsmdsmkm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlphaName",
                table: "Countries");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Countries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$ssftLF36dtXINI7yYsSOweS2NwGRwzMF/IofgYfG2yheLUUNgVo5O");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "Countries");

            migrationBuilder.AddColumn<string>(
                name: "AlphaName",
                table: "Countries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$VIi3EkFB674ZC1GU.dJanOETO7H9Kjd6u7znYHPl324oFevlYYVD2");
        }
    }
}
