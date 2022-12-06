using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class dskmskmskmdk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DashboardViews",
                columns: new[] { "Id", "Name", "ViewPath" },
                values: new object[,]
                {
                    //{ 35, "AccountSubscription", "AccountSubscription" },
                    //{ 36, "Payment", "Payment" },
                    { 38, "AccountTeamPlayer", "AccountTeamPlayer" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$mlqO9w9EB78bBc0xK83aAOr07DN4rdhlSMcWjDoJyk4XLWFEdKIWO");

            migrationBuilder.InsertData(
                table: "AdministrationRolePremissions",
                columns: new[] { "Id", "Fk_DashboardAccessLevel", "Fk_DashboardAdministrationRole", "Fk_DashboardView" },
                values: new object[,]
                {
                    //{ 35, 1, 1, 35 },
                    //{ 36, 1, 1, 36 },
                    { 37, 1, 1, 38 }
                });

            migrationBuilder.InsertData(
                table: "DashboardViewLang",
                columns: new[] { "Id", "Fk_Source", "Name" },
                values: new object[,]
                {
                    //{ 35, 35, "AccountSubscription" },
                    //{ 36, 36, "Payment" },
                    { 38, 38, "AccountTeamPlayer" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "DashboardViewLang",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "DashboardViewLang",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "DashboardViewLang",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "DashboardViews",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "DashboardViews",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "DashboardViews",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$kKvISOQ7puPwJSw1xbGkkeWKboy9QFRm5JHdFNvqeJ/8oegEHWDPm");
        }
    }
}
