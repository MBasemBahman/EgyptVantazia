using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class mkmdsmd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<string>(
                name: "ShortName",
                table: "Teams",
                type: "nvarchar(max)",
                nullable: true);

            _ = migrationBuilder.AddColumn<string>(
                name: "ShortName",
                table: "TeamLang",
                type: "nvarchar(max)",
                nullable: true);

            _ = migrationBuilder.AddColumn<string>(
                name: "ShortName",
                table: "PlayerPositions",
                type: "nvarchar(max)",
                nullable: true);

            _ = migrationBuilder.AddColumn<string>(
                name: "ShortName",
                table: "PlayerPositionLang",
                type: "nvarchar(max)",
                nullable: true);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$oj/7HhgMcFyY9DnPke/wt.hP57hK5KbdO5tSf7rYonOHx2CO8cOLW");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "ShortName",
                table: "Teams");

            _ = migrationBuilder.DropColumn(
                name: "ShortName",
                table: "TeamLang");

            _ = migrationBuilder.DropColumn(
                name: "ShortName",
                table: "PlayerPositions");

            _ = migrationBuilder.DropColumn(
                name: "ShortName",
                table: "PlayerPositionLang");

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$ssftLF36dtXINI7yYsSOweS2NwGRwzMF/IofgYfG2yheLUUNgVo5O");
        }
    }
}
