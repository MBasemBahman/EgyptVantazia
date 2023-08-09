using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class Mkdskdm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DashboardViews",
                columns: new[] { "Id", "Name", "ViewPath" },
                values: new object[] { 48, "PlayerTransfer", "PlayerTransfer" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$BK2sExts0C8aB6Pu9gGCS.FlVayEpgDC/zB3Z7r6olxVWsIcccELu");

            migrationBuilder.InsertData(
                table: "AdministrationRolePremissions",
                columns: new[] { "Id", "Fk_DashboardAccessLevel", "Fk_DashboardAdministrationRole", "Fk_DashboardView" },
                values: new object[] { 47, 1, 1, 48 });

            migrationBuilder.InsertData(
                table: "DashboardViewLang",
                columns: new[] { "Id", "Fk_Source", "Name" },
                values: new object[] { 48, 48, "PlayerTransfer" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "DashboardViewLang",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "DashboardViews",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$nIzGuQlKn38uhphOTTzsze4f7bzXYhfU/Fk0OIqmuxUcbnTK21OD6");
        }
    }
}
