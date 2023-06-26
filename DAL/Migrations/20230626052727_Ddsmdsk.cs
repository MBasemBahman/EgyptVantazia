using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class Ddsmdsk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Fk_Team",
                table: "MatchStatisticScores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            //migrationBuilder.UpdateData(
            //    table: "AdministrationRolePremissions",
            //    keyColumn: "Id",
            //    keyValue: 40,
            //    column: "Fk_DashboardView",
            //    value: 41);

            //migrationBuilder.UpdateData(
            //    table: "AdministrationRolePremissions",
            //    keyColumn: "Id",
            //    keyValue: 41,
            //    column: "Fk_DashboardView",
            //    value: 42);

            //migrationBuilder.UpdateData(
            //    table: "AdministrationRolePremissions",
            //    keyColumn: "Id",
            //    keyValue: 42,
            //    column: "Fk_DashboardView",
            //    value: 43);

            //migrationBuilder.UpdateData(
            //    table: "AdministrationRolePremissions",
            //    keyColumn: "Id",
            //    keyValue: 43,
            //    column: "Fk_DashboardView",
            //    value: 44);

            //migrationBuilder.InsertData(
            //    table: "AdministrationRolePremissions",
            //    columns: new[] { "Id", "Fk_DashboardAccessLevel", "Fk_DashboardAdministrationRole", "Fk_DashboardView" },
            //    values: new object[,]
            //    {
            //        { 44, 1, 1, 45 },
            //        { 45, 1, 1, 46 }
            //    });

            //migrationBuilder.InsertData(
            //    table: "DashboardViews",
            //    columns: new[] { "Id", "Name", "ViewPath" },
            //    values: new object[,]
            //    {
            //        { 41, "Mark", "Mark" },
            //        { 42, "PlayerMark", "PlayerMark" }
            //    });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$ClzDEu1dLEZiCzwXki8bq.tfFRa1hnebuokvpcu/pOxNzIh0x1M5i");

            //migrationBuilder.InsertData(
            //    table: "DashboardViewLang",
            //    columns: new[] { "Id", "Fk_Source", "Name" },
            //    values: new object[,]
            //    {
            //        { 41, 41, "Mark" },
            //        { 42, 42, "PlayerMark" }
            //    });

            migrationBuilder.CreateIndex(
                name: "IX_MatchStatisticScores_Fk_Team",
                table: "MatchStatisticScores",
                column: "Fk_Team");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchStatisticScores_Teams_Fk_Team",
                table: "MatchStatisticScores",
                column: "Fk_Team",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchStatisticScores_Teams_Fk_Team",
                table: "MatchStatisticScores");

            migrationBuilder.DropIndex(
                name: "IX_MatchStatisticScores_Fk_Team",
                table: "MatchStatisticScores");

            migrationBuilder.DeleteData(
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "DashboardViewLang",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "DashboardViewLang",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "DashboardViews",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "DashboardViews",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DropColumn(
                name: "Fk_Team",
                table: "MatchStatisticScores");

            migrationBuilder.UpdateData(
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 40,
                column: "Fk_DashboardView",
                value: 43);

            migrationBuilder.UpdateData(
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 41,
                column: "Fk_DashboardView",
                value: 44);

            migrationBuilder.UpdateData(
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 42,
                column: "Fk_DashboardView",
                value: 45);

            migrationBuilder.UpdateData(
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 43,
                column: "Fk_DashboardView",
                value: 46);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$Di9iiT3XXeWCumtA8UDEv.2FIdoAE7/E9zkrqtadHQExNmZ27aYA.");
        }
    }
}
