using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class ffhfhhfbvb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DashboardViews",
                columns: new[] { "Id", "Name", "ViewPath" },
                values: new object[,]
                {
                    { 43, "PromoCode", "PromoCode" },
                    { 44, "StatisticScore", "StatisticScore" },
                    { 45, "StatisticCategory", "StatisticCategory" },
                    { 46, "MatchStatisticScore", "MatchStatisticScore" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$Di9iiT3XXeWCumtA8UDEv.2FIdoAE7/E9zkrqtadHQExNmZ27aYA.");

            migrationBuilder.InsertData(
                table: "AdministrationRolePremissions",
                columns: new[] { "Id", "Fk_DashboardAccessLevel", "Fk_DashboardAdministrationRole", "Fk_DashboardView" },
                values: new object[,]
                {
                    { 40, 1, 1, 43 },
                    { 41, 1, 1, 44 },
                    { 42, 1, 1, 45 },
                    { 43, 1, 1, 46 }
                });

            migrationBuilder.InsertData(
                table: "DashboardViewLang",
                columns: new[] { "Id", "Fk_Source", "Name" },
                values: new object[,]
                {
                    { 43, 43, "PromoCode" },
                    { 44, 44, "StatisticScore" },
                    { 45, 45, "StatisticCategory" },
                    { 46, 46, "MatchStatisticScore" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "DashboardViewLang",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "DashboardViewLang",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "DashboardViewLang",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "DashboardViewLang",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "DashboardViews",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "DashboardViews",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "DashboardViews",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "DashboardViews",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$ZrR6pi9sE43besV7QMd6G.nNs8DWaAXgo8kSjU51zhW.UlSWiKttW");
        }
    }
}
