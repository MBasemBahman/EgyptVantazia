using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class mdkmdkm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TiktokUrl",
                table: "AppAbout",
                type: "nvarchar(max)",
                nullable: true);

            //migrationBuilder.InsertData(
            //    table: "DashboardViews",
            //    columns: new[] { "Id", "Name", "ViewPath" },
            //    values: new object[] { 40, "Notification", "Notification" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$319o9zp.mamiWdk1X9IbBO0i1QqrZiyfh1VAK2iq345EOcpHYW33m");

            //migrationBuilder.InsertData(
            //    table: "AdministrationRolePremissions",
            //    columns: new[] { "Id", "Fk_DashboardAccessLevel", "Fk_DashboardAdministrationRole", "Fk_DashboardView" },
            //    values: new object[] { 39, 1, 1, 40 });

            //migrationBuilder.InsertData(
            //    table: "DashboardViewLang",
            //    columns: new[] { "Id", "Fk_Source", "Name" },
            //    values: new object[] { 40, 40, "Notification" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "DashboardViewLang",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "DashboardViews",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DropColumn(
                name: "TiktokUrl",
                table: "AppAbout");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$aJWdF38.aGVQRUx0s8vhQeeRGDTkTmuqax1aXIPedI7YbWj/cikdK");
        }
    }
}
