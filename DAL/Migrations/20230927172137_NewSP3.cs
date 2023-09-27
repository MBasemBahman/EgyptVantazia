using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class NewSP3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string procedureScript = File.ReadAllText("../BaseDB/SqlScripts/StoredProcedure/PlayerSeasonScoreState.Sql");
            migrationBuilder.Sql(procedureScript);

            string procedureScript2 = File.ReadAllText("../BaseDB/SqlScripts/StoredProcedure/PlayerGameWeakScoreState.Sql");
            migrationBuilder.Sql(procedureScript2);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$YRYMHGRobxVTzw/RSeqFOeeDyn7hjr5QpXV36hcC3nHRCLkVOBuWC");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$tK5woovuN5Se2k5WpqbOheZCqOLfNtfPH9re35SCYw.KuPeHh2lz6");
        }
    }
}
