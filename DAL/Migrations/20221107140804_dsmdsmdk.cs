using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class dsmdsmdk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AlterColumn<string>(
                name: "RefCode",
                table: "Accounts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            _ = migrationBuilder.AddColumn<int>(
                name: "RefCodeCount",
                table: "Accounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$4xnTUS1iZYWrr1xP8LhW3ONXSO.7Fa8trPIKjtFx3.7Obiov4v18.");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Accounts_RefCode",
                table: "Accounts",
                column: "RefCode",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropIndex(
                name: "IX_Accounts_RefCode",
                table: "Accounts");

            _ = migrationBuilder.DropColumn(
                name: "RefCodeCount",
                table: "Accounts");

            _ = migrationBuilder.AlterColumn<string>(
                name: "RefCode",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$aCXmfCoX9W0PaeeP0pIHkujLbS89YdejCEaDALk0HZufed5cY78v.");
        }
    }
}
