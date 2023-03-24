using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class msmdsmds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SecondJobId",
                table: "GameWeaks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$A8zQ825L6ljzlO1RqK/9m.j1lTThHS5cWejyhpuC54sgKVsNzSoyC");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecondJobId",
                table: "GameWeaks");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$4pWep1S1mCbwUZh8CWEJPOrKX7X5lnyX7TncSRmzekrtFfi6p1RjG");
        }
    }
}
