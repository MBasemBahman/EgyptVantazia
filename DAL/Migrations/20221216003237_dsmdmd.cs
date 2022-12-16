using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class dsmdmd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOut",
                table: "PlayerGameWeakScores",
                type: "bit",
                nullable: true);

            //migrationBuilder.InsertData(
            //    table: "DashboardViews",
            //    columns: new[] { "Id", "Name", "ViewPath" },
            //    values: new object[] { 39, "AccountTeamGameWeak", "AccountTeamGameWeak" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$dqJUfqHxwqUU6ETEUWI3b.LGpPynQevTYCiw8HXZh2Y8v6tb6mQfq");

            //migrationBuilder.InsertData(
            //    table: "AdministrationRolePremissions",
            //    columns: new[] { "Id", "Fk_DashboardAccessLevel", "Fk_DashboardAdministrationRole", "Fk_DashboardView" },
            //    values: new object[] { 38, 1, 1, 39 });

            //migrationBuilder.InsertData(
            //    table: "DashboardViewLang",
            //    columns: new[] { "Id", "Fk_Source", "Name" },
            //    values: new object[] { 39, 39, "AccountTeamGameWeak" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "DashboardViewLang",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "DashboardViews",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DropColumn(
                name: "IsOut",
                table: "PlayerGameWeakScores");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$5jPv9MBzdyfhk/Uiu3Q9J.ZFlFOYbK9.E9pnANrff8MaHVKaaBGIm");
        }
    }
}
