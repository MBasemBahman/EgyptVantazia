using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class mkmdkms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "_365_PlayerId",
                table: "PlayerGameWeaks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$6P7xlkRdBB/4k6g2wZwQ.OV.KXsv59s6I3618vd8Ml.CdnxFe0Wlu");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "_365_PlayerId",
                table: "PlayerGameWeaks");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$dqJUfqHxwqUU6ETEUWI3b.LGpPynQevTYCiw8HXZh2Y8v6tb6mQfq");
        }
    }
}
