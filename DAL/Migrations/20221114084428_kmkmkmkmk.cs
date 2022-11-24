using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class kmkmkmkmk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<string>(
                name: "JobId",
                table: "TeamGameWeaks",
                type: "nvarchar(max)",
                nullable: true);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$HrIihtkxBetu/96ZjUG/n.C5srm3FQBBNxjh1NA/PMDKJRqWUE0Q2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "JobId",
                table: "TeamGameWeaks");

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$gky4JNv93a4BHAVNKRJDKu8V94aQL9/CDH7BH3L2myzi85xydXLYK");
        }
    }
}
