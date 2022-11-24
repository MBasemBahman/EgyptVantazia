using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class dskmdskmd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<int>(
                name: "Cost",
                table: "Subscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.AddColumn<int>(
                name: "Discount",
                table: "Subscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.AddColumn<bool>(
                name: "ForAction",
                table: "Subscriptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            _ = migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Subscriptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$q7VdwMZCq5qM5KSaioaelOvO95xJV7CdYP0al7zyUyXHFw3PvJKpW");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "Cost",
                table: "Subscriptions");

            _ = migrationBuilder.DropColumn(
                name: "Discount",
                table: "Subscriptions");

            _ = migrationBuilder.DropColumn(
                name: "ForAction",
                table: "Subscriptions");

            _ = migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Subscriptions");

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$oj/7HhgMcFyY9DnPke/wt.hP57hK5KbdO5tSf7rYonOHx2CO8cOLW");
        }
    }
}
