using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class wewqww : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanDeploy",
                table: "DashboardAdministrators",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$zDoC99XJ1xJJu83Y3gCWxOWr.1U8kWw85sETRMDONqRtqf6eoa.vi");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanDeploy",
                table: "DashboardAdministrators");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$A8zQ825L6ljzlO1RqK/9m.j1lTThHS5cWejyhpuC54sgKVsNzSoyC");
        }
    }
}
