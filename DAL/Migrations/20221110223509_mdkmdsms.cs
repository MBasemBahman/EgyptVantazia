using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class mdkmdsms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "AlphaName",
                table: "Countries");

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$OQ65cVR7HJxryE2Duj06TuOSnacyM0e5nXX0WOOdovtjTpE2Ewe7S");
        }
    }
}
