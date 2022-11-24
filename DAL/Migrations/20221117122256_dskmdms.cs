using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class dskmdms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Subscriptions",
                type: "nvarchar(max)",
                nullable: true);

            _ = migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SubscriptionLang",
                type: "nvarchar(max)",
                nullable: true);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$VcNslDXA/gr9ZuFHXIT2aewi/CxXZWc6u2UtEnH3STYxadEiqpw5u");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "Description",
                table: "Subscriptions");

            _ = migrationBuilder.DropColumn(
                name: "Description",
                table: "SubscriptionLang");

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$zTd/bz1p63mNthlx4OmFueeGQQeLP8x6xeicpo5qo.Dp8Nc1PLKAK");
        }
    }
}
