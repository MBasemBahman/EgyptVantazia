using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class dsdskmdmd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "Position",
                table: "PlayerGameWeaks");

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$zTd/bz1p63mNthlx4OmFueeGQQeLP8x6xeicpo5qo.Dp8Nc1PLKAK");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "PlayerGameWeaks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$HrIihtkxBetu/96ZjUG/n.C5srm3FQBBNxjh1NA/PMDKJRqWUE0Q2");
        }
    }
}
