using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class dskfkkjfk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "JobId",
                table: "TeamGameWeaks",
                newName: "LastUpdateId");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$6HsqyOA0Ay0QQpfYb0I/GecivhDXcLkoNjOXYA800jDHgztBXbPQO");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastUpdateId",
                table: "TeamGameWeaks",
                newName: "JobId");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$6P7xlkRdBB/4k6g2wZwQ.OV.KXsv59s6I3618vd8Ml.CdnxFe0Wlu");
        }
    }
}
