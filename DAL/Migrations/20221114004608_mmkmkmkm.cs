using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class mmkmkmkm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<string>(
                name: "JobId",
                table: "GameWeaks",
                type: "nvarchar(max)",
                nullable: true);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$gky4JNv93a4BHAVNKRJDKu8V94aQL9/CDH7BH3L2myzi85xydXLYK");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "JobId",
                table: "GameWeaks");

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$ApDIMnqtUCCFNqAabtaL0ukcgFx0FKfWOguWadNPh2M2WPcuAy.wa");
        }
    }
}
