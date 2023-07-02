using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class mksmks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Seasons_Fk_Season",
                table: "Accounts");

            migrationBuilder.AlterColumn<int>(
                name: "Fk_Season",
                table: "Accounts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$aRAD7exKm6eg4h.ztdYdE.pb0WYxUEgr1oL7Jn7oV/BlU2Ru6iF36");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Seasons_Fk_Season",
                table: "Accounts",
                column: "Fk_Season",
                principalTable: "Seasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Seasons_Fk_Season",
                table: "Accounts");

            migrationBuilder.AlterColumn<int>(
                name: "Fk_Season",
                table: "Accounts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$99xiOyLm6HlqroQnRKasDOFSF4ffQtXOWy5VKV1z4aLzkxkhej/1S");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Seasons_Fk_Season",
                table: "Accounts",
                column: "Fk_Season",
                principalTable: "Seasons",
                principalColumn: "Id");
        }
    }
}
