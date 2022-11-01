using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class msmdkmk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Order",
                table: "AccountTeamPlayerGameWeaks",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_Account = table.Column<int>(type: "int", nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Accounts_Fk_Account",
                        column: x => x.Fk_Account,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            //migrationBuilder.InsertData(
            //    table: "DashboardViews",
            //    columns: new[] { "Id", "Name", "ViewPath" },
            //    values: new object[,]
            //    {
            //        { 32, "ScoreState", "ScoreState" },
            //        { 33, "PlayerGameWeakScoreState", "PlayerGameWeakScoreState" },
            //        { 34, "PlayerSeasonScoreState", "PlayerSeasonScoreState" }
            //    });

            //migrationBuilder.InsertData(
            //    table: "TeamPlayerTypes",
            //    columns: new[] { "Id", "LastModifiedBy", "Name" },
            //    values: new object[] { 3, null, "Normal" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$NTEuwaxMiEQTk5aho2yNn.kh5V.wb.gv00LHryvtqysB0PqRM0yPa");

            //migrationBuilder.InsertData(
            //    table: "AdministrationRolePremissions",
            //    columns: new[] { "Id", "Fk_DashboardAccessLevel", "Fk_DashboardAdministrationRole", "Fk_DashboardView" },
            //    values: new object[,]
            //    {
            //        { 32, 1, 1, 32 },
            //        { 33, 1, 1, 33 },
            //        { 34, 1, 1, 34 }
            //    });

            //migrationBuilder.InsertData(
            //    table: "DashboardViewLang",
            //    columns: new[] { "Id", "Fk_Source", "Name" },
            //    values: new object[,]
            //    {
            //        { 32, 32, "ScoreState" },
            //        { 33, 33, "PlayerGameWeakScoreState" },
            //        { 34, 34, "PlayerSeasonScoreState" }
            //    });

            //migrationBuilder.InsertData(
            //    table: "TeamPlayerTypeLang",
            //    columns: new[] { "Id", "Fk_Source", "Name" },
            //    values: new object[] { 3, 3, "Normal" });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_Fk_Account",
                table: "Payments",
                column: "Fk_Account");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DeleteData(
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "AdministrationRolePremissions",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "DashboardViewLang",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "DashboardViewLang",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "DashboardViewLang",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "TeamPlayerTypeLang",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "DashboardViews",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "DashboardViews",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "DashboardViews",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "TeamPlayerTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AlterColumn<bool>(
                name: "Order",
                table: "AccountTeamPlayerGameWeaks",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$HcVj022UbsbVXwm4gIMyAenrpC/RXdN9G.uYSgIty3Yg5NObCw2WC");
        }
    }
}
