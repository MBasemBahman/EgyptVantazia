using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class ddhhhcxvbss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "_365_AfterGameStartId",
                table: "Seasons");

            _ = migrationBuilder.InsertData(
                table: "DashboardViews",
                columns: new[] { "Id", "Name", "ViewPath" },
                values: new object[,]
                {
                    { 18, "PlayerPosition", "PlayerPosition" },
                    { 19, "Player", "Player" },
                    { 20, "PrivateLeague", "PrivateLeague" },
                    { 21, "PrivateLeagueMember", "PrivateLeagueMember" },
                    { 22, "ScoreType", "ScoreType" },
                    { 23, "TeamPlayerType", "TeamPlayerType" },
                    { 24, "PlayerGameWeak", "PlayerGameWeak" },
                    { 25, "PlayerGameWeakScore", "PlayerGameWeakScore" },
                    { 26, "Season", "Season" },
                    { 27, "GameWeak", "GameWeak" },
                    { 28, "TeamGameWeak", "TeamGameWeak" },
                    { 29, "Standings", "Standings" }
                });

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$p6ldbBMB3Qfd7qpKsJuhc.kce02MjEyJRl1aK0wiVyhcYMOd.2/Mu");

            _ = migrationBuilder.InsertData(
                table: "AdministrationRolePremissions",
                columns: new[] { "Id", "Fk_DashboardAccessLevel", "Fk_DashboardAdministrationRole", "Fk_DashboardView" },
                values: new object[,]
                {
                    { 18, 1, 1, 18 },
                    { 19, 1, 1, 19 },
                    { 20, 1, 1, 20 },
                    { 21, 1, 1, 21 },
                    { 22, 1, 1, 22 },
                    { 23, 1, 1, 23 },
                    { 24, 1, 1, 24 },
                    { 25, 1, 1, 25 },
                    { 26, 1, 1, 26 },
                    { 27, 1, 1, 27 },
                    { 28, 1, 1, 28 },
                    { 29, 1, 1, 29 }
                });

            _ = migrationBuilder.InsertData(
                table: "DashboardViewLang",
                columns: new[] { "Id", "Fk_Source", "Name" },
                values: new object[,]
                {
                    { 18, 18, "PlayerPosition" },
                    { 19, 19, "Player" },
                    { 20, 20, "PrivateLeague" },
                    { 21, 21, "PrivateLeagueMember" },
                    { 22, 22, "ScoreType" },
                    { 23, 23, "TeamPlayerType" },
                    { 24, 24, "PlayerGameWeak" },
                    { 25, 25, "PlayerGameWeakScore" },
                    { 26, 26, "Season" },
                    { 27, 27, "GameWeak" },
                    { 28, 28, "TeamGameWeak" },
                    { 29, 29, "Standings" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DeleteData(
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 18);

            _ = migrationBuilder.DeleteData(
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 19);

            _ = migrationBuilder.DeleteData(
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 20);

            _ = migrationBuilder.DeleteData(
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 21);

            _ = migrationBuilder.DeleteData(
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 22);

            _ = migrationBuilder.DeleteData(
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 23);

            _ = migrationBuilder.DeleteData(
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 24);

            _ = migrationBuilder.DeleteData(
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 25);

            _ = migrationBuilder.DeleteData(
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 26);

            _ = migrationBuilder.DeleteData(
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 27);

            _ = migrationBuilder.DeleteData(
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 28);

            _ = migrationBuilder.DeleteData(
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 29);

            _ = migrationBuilder.DeleteData(
                table: "DashboardViewLang",
                keyColumn: "Id",
                keyValue: 18);

            _ = migrationBuilder.DeleteData(
                table: "DashboardViewLang",
                keyColumn: "Id",
                keyValue: 19);

            _ = migrationBuilder.DeleteData(
                table: "DashboardViewLang",
                keyColumn: "Id",
                keyValue: 20);

            _ = migrationBuilder.DeleteData(
                table: "DashboardViewLang",
                keyColumn: "Id",
                keyValue: 21);

            _ = migrationBuilder.DeleteData(
                table: "DashboardViewLang",
                keyColumn: "Id",
                keyValue: 22);

            _ = migrationBuilder.DeleteData(
                table: "DashboardViewLang",
                keyColumn: "Id",
                keyValue: 23);

            _ = migrationBuilder.DeleteData(
                table: "DashboardViewLang",
                keyColumn: "Id",
                keyValue: 24);

            _ = migrationBuilder.DeleteData(
                table: "DashboardViewLang",
                keyColumn: "Id",
                keyValue: 25);

            _ = migrationBuilder.DeleteData(
                table: "DashboardViewLang",
                keyColumn: "Id",
                keyValue: 26);

            _ = migrationBuilder.DeleteData(
                table: "DashboardViewLang",
                keyColumn: "Id",
                keyValue: 27);

            _ = migrationBuilder.DeleteData(
                table: "DashboardViewLang",
                keyColumn: "Id",
                keyValue: 28);

            _ = migrationBuilder.DeleteData(
                table: "DashboardViewLang",
                keyColumn: "Id",
                keyValue: 29);

            _ = migrationBuilder.DeleteData(
                table: "DashboardViews",
                keyColumn: "Id",
                keyValue: 18);

            _ = migrationBuilder.DeleteData(
                table: "DashboardViews",
                keyColumn: "Id",
                keyValue: 19);

            _ = migrationBuilder.DeleteData(
                table: "DashboardViews",
                keyColumn: "Id",
                keyValue: 20);

            _ = migrationBuilder.DeleteData(
                table: "DashboardViews",
                keyColumn: "Id",
                keyValue: 21);

            _ = migrationBuilder.DeleteData(
                table: "DashboardViews",
                keyColumn: "Id",
                keyValue: 22);

            _ = migrationBuilder.DeleteData(
                table: "DashboardViews",
                keyColumn: "Id",
                keyValue: 23);

            _ = migrationBuilder.DeleteData(
                table: "DashboardViews",
                keyColumn: "Id",
                keyValue: 24);

            _ = migrationBuilder.DeleteData(
                table: "DashboardViews",
                keyColumn: "Id",
                keyValue: 25);

            _ = migrationBuilder.DeleteData(
                table: "DashboardViews",
                keyColumn: "Id",
                keyValue: 26);

            _ = migrationBuilder.DeleteData(
                table: "DashboardViews",
                keyColumn: "Id",
                keyValue: 27);

            _ = migrationBuilder.DeleteData(
                table: "DashboardViews",
                keyColumn: "Id",
                keyValue: 28);

            _ = migrationBuilder.DeleteData(
                table: "DashboardViews",
                keyColumn: "Id",
                keyValue: 29);

            _ = migrationBuilder.AddColumn<string>(
                name: "_365_AfterGameStartId",
                table: "Seasons",
                type: "nvarchar(max)",
                nullable: true);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$.BAbheoXcd92wJSio8Nt0.4sK3GMH6aGk5H4VM9DXd5eN4pw9Rgwe");
        }
    }
}
