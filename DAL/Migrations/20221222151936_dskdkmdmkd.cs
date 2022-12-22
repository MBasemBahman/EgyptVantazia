using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class dskdkmdmkd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowInvite",
                table: "AppAbout",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowPayment",
                table: "AppAbout",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$kjqA/aFrG7VRMqnc51YP0.5of8/wGMvS1FeCxEORxe3Ao0mHPn/JS");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowInvite",
                table: "AppAbout");

            migrationBuilder.DropColumn(
                name: "ShowPayment",
                table: "AppAbout");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$0WQKkEh5/kccZ9iT83/TFu3GcKBgN1CYeYet0HqNXTHStVUShRhay");
        }
    }
}
