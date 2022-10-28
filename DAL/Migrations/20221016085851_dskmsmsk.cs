using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class dskmsmsk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<bool>(
                name: "HavePoints",
                table: "ScoreTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            _ = migrationBuilder.AddColumn<bool>(
                name: "IsEvent",
                table: "ScoreTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            _ = migrationBuilder.AddColumn<string>(
                name: "_365_EventTypeId",
                table: "ScoreTypes",
                type: "nvarchar(max)",
                nullable: true);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$1eGvaPpOk/MSPRKhYzPEeeTkbUmQQhhfArMUF.ZSkSiULQpvxiwvW");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "HavePoints",
                table: "ScoreTypes");

            _ = migrationBuilder.DropColumn(
                name: "IsEvent",
                table: "ScoreTypes");

            _ = migrationBuilder.DropColumn(
                name: "_365_EventTypeId",
                table: "ScoreTypes");

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$rylYIcKhXwliYYb6.1/4guRTZBfSIPztuQvHuFhUUPHaYhC.e2tM2");
        }
    }
}
