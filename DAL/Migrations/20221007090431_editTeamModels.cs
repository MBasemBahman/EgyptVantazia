using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class editTeamModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.AddColumn<string>(
                name: "ShortName",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            _ = migrationBuilder.AddColumn<string>(
                name: "ShortName",
                table: "PlayerLang",
                type: "nvarchar(max)",
                nullable: true);

            //migrationBuilder.InsertData(
            //    table: "DashboardViews",
            //    columns: new[] { "Id", "Name", "ViewPath" },
            //    values: new object[,]
            //    {
            //        { 10, "Account", "Account" },
            //        { 11, "Country", "Country" },
            //        { 12, "DBLogs", "DBLogs" },
            //        { 13, "AppAbout", "AppAbout" }
            //    });

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$FfIkZkJSQLK552mTQhlDsuqA1MKVL8j9GNVRljjyc1Fza/nIVQw4C");

            //migrationBuilder.InsertData(
            //    table: "AdministrationRolePremissions",
            //    columns: new[] { "Id", "Fk_DashboardAccessLevel", "Fk_DashboardAdministrationRole", "Fk_DashboardView" },
            //    values: new object[,]
            //    {
            //        { 10, 1, 1, 10 },
            //        { 11, 1, 1, 11 },
            //        { 12, 1, 1, 12 },
            //        { 13, 1, 1, 13 }
            //    });

            //migrationBuilder.InsertData(
            //    table: "DashboardViewLang",
            //    columns: new[] { "Id", "Fk_Source", "Name" },
            //    values: new object[,]
            //    {
            //        { 10, 10, "Account" },
            //        { 11, 11, "Country" },
            //        { 12, 12, "DBLogs" },
            //        { 13, 13, "AppAbout" }
            //    });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DeleteData(
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 10);

            _ = migrationBuilder.DeleteData(
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 11);

            _ = migrationBuilder.DeleteData(
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 12);

            _ = migrationBuilder.DeleteData(
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 13);

            _ = migrationBuilder.DeleteData(
                table: "DashboardViewLang",
                keyColumn: "Id",
                keyValue: 10);

            _ = migrationBuilder.DeleteData(
                table: "DashboardViewLang",
                keyColumn: "Id",
                keyValue: 11);

            _ = migrationBuilder.DeleteData(
                table: "DashboardViewLang",
                keyColumn: "Id",
                keyValue: 12);

            _ = migrationBuilder.DeleteData(
                table: "DashboardViewLang",
                keyColumn: "Id",
                keyValue: 13);

            _ = migrationBuilder.DeleteData(
                table: "DashboardViews",
                keyColumn: "Id",
                keyValue: 10);

            _ = migrationBuilder.DeleteData(
                table: "DashboardViews",
                keyColumn: "Id",
                keyValue: 11);

            _ = migrationBuilder.DeleteData(
                table: "DashboardViews",
                keyColumn: "Id",
                keyValue: 12);

            _ = migrationBuilder.DeleteData(
                table: "DashboardViews",
                keyColumn: "Id",
                keyValue: 13);

            _ = migrationBuilder.DropColumn(
                name: "Age",
                table: "Players");

            _ = migrationBuilder.DropColumn(
                name: "ShortName",
                table: "Players");

            _ = migrationBuilder.DropColumn(
                name: "ShortName",
                table: "PlayerLang");

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$TOhUTTK0HXBZXzlAAS0Cgu2eF1GCJbsC8OgnBkcLiaJQBYfaOkQBi");
        }
    }
}
