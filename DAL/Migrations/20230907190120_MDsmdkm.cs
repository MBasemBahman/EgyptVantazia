using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class MDsmdkm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Fk_CommunicationStatus",
                table: "AccountTeams",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CommunicationStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunicationStatuses", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$51F6h0sifKnZ4RzT/QLE7uS4J5xwDzSi.d6TGf8OjewRkTX33UQHq");

            migrationBuilder.CreateIndex(
                name: "IX_AccountTeams_Fk_CommunicationStatus",
                table: "AccountTeams",
                column: "Fk_CommunicationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationStatuses_Name",
                table: "CommunicationStatuses",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountTeams_CommunicationStatuses_Fk_CommunicationStatus",
                table: "AccountTeams",
                column: "Fk_CommunicationStatus",
                principalTable: "CommunicationStatuses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountTeams_CommunicationStatuses_Fk_CommunicationStatus",
                table: "AccountTeams");

            migrationBuilder.DropTable(
                name: "CommunicationStatuses");

            migrationBuilder.DropIndex(
                name: "IX_AccountTeams_Fk_CommunicationStatus",
                table: "AccountTeams");

            migrationBuilder.DropColumn(
                name: "Fk_CommunicationStatus",
                table: "AccountTeams");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$8ETip.gH3CKIcGKLVfkkAuwP0iVbCsW89KVM2xWP8wt/9laQlLCi6");
        }
    }
}
