using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class msmdskmds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TeamGameWeaks_Fk_Away_Fk_Home_Fk_GameWeak",
                table: "TeamGameWeaks");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$ZrR6pi9sE43besV7QMd6G.nNs8DWaAXgo8kSjU51zhW.UlSWiKttW");

            migrationBuilder.CreateIndex(
                name: "IX_TeamGameWeaks_Fk_Away",
                table: "TeamGameWeaks",
                column: "Fk_Away");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TeamGameWeaks_Fk_Away",
                table: "TeamGameWeaks");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$sSVeh3FqqihR/nHFtkuQL.AHCuTryWKOOsnmDss.xB.ghjkBadBPO");

            migrationBuilder.CreateIndex(
                name: "IX_TeamGameWeaks_Fk_Away_Fk_Home_Fk_GameWeak",
                table: "TeamGameWeaks",
                columns: new[] { "Fk_Away", "Fk_Home", "Fk_GameWeak" },
                unique: true);
        }
    }
}
