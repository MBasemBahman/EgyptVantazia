﻿using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class dkdkdcv : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.InsertData(
                table: "DashboardViews",
                columns: new[] { "Id", "Name", "ViewPath" },
                values: new object[,]
                {
                    { 10, "Account", "Account" },
                    { 11, "Country", "Country" },
                    { 12, "DBLogs", "DBLogs" },
                    { 13, "AppAbout", "AppAbout" },
                    { 14, "Team", "Team" },
                    { 15, "News", "News" },
                    { 16, "NewsAttachment", "NewsAttachment" },
                    { 17, "Sponsor", "Sponsor" }
                });

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$rJE24T6rOfSJdhAVxoAvdO9NldxjvjAC9VMDjvWQGxkbAlRTeR0Li");

            _ = migrationBuilder.InsertData(
                table: "AdministrationRolePremissions",
                columns: new[] { "Id", "Fk_DashboardAccessLevel", "Fk_DashboardAdministrationRole", "Fk_DashboardView" },
                values: new object[,]
                {
                    { 10, 1, 1, 10 },
                    { 11, 1, 1, 11 },
                    { 12, 1, 1, 12 },
                    { 13, 1, 1, 13 },
                    { 14, 1, 1, 14 },
                    { 15, 1, 1, 15 },
                    { 16, 1, 1, 16 },
                    { 17, 1, 1, 17 }
                });

            _ = migrationBuilder.InsertData(
                table: "DashboardViewLang",
                columns: new[] { "Id", "Fk_Source", "Name" },
                values: new object[,]
                {
                    { 10, 10, "Account" },
                    { 11, 11, "Country" },
                    { 12, 12, "DBLogs" },
                    { 13, 13, "AppAbout" },
                    { 14, 14, "Team" },
                    { 15, 15, "News" },
                    { 16, 16, "NewsAttachment" },
                    { 17, 17, "Sponsor" }
                });
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
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 14);

            _ = migrationBuilder.DeleteData(
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 15);

            _ = migrationBuilder.DeleteData(
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 16);

            _ = migrationBuilder.DeleteData(
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 17);

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
                table: "DashboardViewLang",
                keyColumn: "Id",
                keyValue: 14);

            _ = migrationBuilder.DeleteData(
                table: "DashboardViewLang",
                keyColumn: "Id",
                keyValue: 15);

            _ = migrationBuilder.DeleteData(
                table: "DashboardViewLang",
                keyColumn: "Id",
                keyValue: 16);

            _ = migrationBuilder.DeleteData(
                table: "DashboardViewLang",
                keyColumn: "Id",
                keyValue: 17);

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

            _ = migrationBuilder.DeleteData(
                table: "DashboardViews",
                keyColumn: "Id",
                keyValue: 14);

            _ = migrationBuilder.DeleteData(
                table: "DashboardViews",
                keyColumn: "Id",
                keyValue: 15);

            _ = migrationBuilder.DeleteData(
                table: "DashboardViews",
                keyColumn: "Id",
                keyValue: 16);

            _ = migrationBuilder.DeleteData(
                table: "DashboardViews",
                keyColumn: "Id",
                keyValue: 17);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$TOhUTTK0HXBZXzlAAS0Cgu2eF1GCJbsC8OgnBkcLiaJQBYfaOkQBi");
        }
    }
}
