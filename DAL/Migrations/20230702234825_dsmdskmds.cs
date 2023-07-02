using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class dsmdskmds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Fk_Season",
                table: "Accounts",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$99xiOyLm6HlqroQnRKasDOFSF4ffQtXOWy5VKV1z4aLzkxkhej/1S");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Fk_Season",
                table: "Accounts",
                column: "Fk_Season");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Seasons_Fk_Season",
                table: "Accounts",
                column: "Fk_Season",
                principalTable: "Seasons",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Seasons_Fk_Season",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_Fk_Season",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Fk_Season",
                table: "Accounts");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$rdqBDWZYVa58OW2kIpQaUOqx/3ggY0A1GYRUlmREvs0X1tWKbxSgu");
        }
    }
}
