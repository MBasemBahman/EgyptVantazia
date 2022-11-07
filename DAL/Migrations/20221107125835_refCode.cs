using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class refCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$aCXmfCoX9W0PaeeP0pIHkujLbS89YdejCEaDALk0HZufed5cY78v.");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountRefCode");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$2CTWHkJ1lAv9Q2iT1EZAo.PHe7pT.bDbdSZGOQ2sJx975zbAJpcea");
        }
    }
}
