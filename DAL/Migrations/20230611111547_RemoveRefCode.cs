using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class RemoveRefCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountRefCode");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_RefCode",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "RefCode",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "RefCodeCount",
                table: "Accounts");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$jfaSOG1mzWkq/J7Xift06OEVQPub5gCdzisvdJRHD61MFSvYT4WdO");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RefCode",
                table: "Accounts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RefCodeCount",
                table: "Accounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AccountRefCode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_NewAccount = table.Column<int>(type: "int", nullable: false),
                    Fk_RefAccount = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountRefCode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountRefCode_Accounts_Fk_NewAccount",
                        column: x => x.Fk_NewAccount,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountRefCode_Accounts_Fk_RefAccount",
                        column: x => x.Fk_RefAccount,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$h5DbOIbHTgXSqKcEkbTSwuDydYXK1rk3dIUscWpfxvkNDCx2DKBZW");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_RefCode",
                table: "Accounts",
                column: "RefCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountRefCode_Fk_NewAccount",
                table: "AccountRefCode",
                column: "Fk_NewAccount",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountRefCode_Fk_NewAccount_Fk_RefAccount",
                table: "AccountRefCode",
                columns: new[] { "Fk_NewAccount", "Fk_RefAccount" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountRefCode_Fk_RefAccount",
                table: "AccountRefCode",
                column: "Fk_RefAccount");
        }
    }
}
