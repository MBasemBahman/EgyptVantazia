using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class ve2dsadsadas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "PlayerMarks");

            migrationBuilder.CreateTable(
                name: "PlayerMarkReasonMatch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_PlayerMark = table.Column<int>(type: "int", nullable: false),
                    Fk_TeamGameWeak = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerMarkReasonMatch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerMarkReasonMatch_PlayerMarks_Fk_PlayerMark",
                        column: x => x.Fk_PlayerMark,
                        principalTable: "PlayerMarks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlayerMarkReasonMatch_TeamGameWeaks_Fk_TeamGameWeak",
                        column: x => x.Fk_TeamGameWeak,
                        principalTable: "TeamGameWeaks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$e.agnY2ZRUWoCKiM77b6G.yL0LkcXECHkKsrMCY72OLD9SdRbVOFy");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMarkReasonMatch_Fk_PlayerMark",
                table: "PlayerMarkReasonMatch",
                column: "Fk_PlayerMark");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMarkReasonMatch_Fk_TeamGameWeak",
                table: "PlayerMarkReasonMatch",
                column: "Fk_TeamGameWeak");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerMarkReasonMatch");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "PlayerMarks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$5a1oluAuudih5q9EYtMCr.suyRpc2wkvJJP3S5LKDUWa6AwqrMz1e");
        }
    }
}
