using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class dskmsmsk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HavePoints",
                table: "ScoreTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEvent",
                table: "ScoreTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "_365_EventTypeId",
                table: "ScoreTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$1eGvaPpOk/MSPRKhYzPEeeTkbUmQQhhfArMUF.ZSkSiULQpvxiwvW");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HavePoints",
                table: "ScoreTypes");

            migrationBuilder.DropColumn(
                name: "IsEvent",
                table: "ScoreTypes");

            migrationBuilder.DropColumn(
                name: "_365_EventTypeId",
                table: "ScoreTypes");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$rylYIcKhXwliYYb6.1/4guRTZBfSIPztuQvHuFhUUPHaYhC.e2tM2");
        }
    }
}
