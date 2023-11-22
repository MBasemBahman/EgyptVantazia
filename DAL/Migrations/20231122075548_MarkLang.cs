using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class MarkLang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayerMarkLang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    Fk_Source = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerMarkLang", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerMarkLang_PlayerMarks_Fk_Source",
                        column: x => x.Fk_Source,
                        principalTable: "PlayerMarks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            //migrationBuilder.UpdateData(
            //    table: "Users",
            //    keyColumn: "Id",
            //    keyValue: 1,
            //    column: "Password",
            //    value: "$2a$11$tc0WdSv56UwZYZaxBh2bHOqr5LC7r2E.WQJtTtWmG0WtTIBp3YPpS");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMarkLang_Fk_Source",
                table: "PlayerMarkLang",
                column: "Fk_Source",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerMarkLang");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$Mhv5cbaKb68F4OzdDd9VQebgM79k7hRiADlk6BXooZrGcZCpKFDhy");
        }
    }
}
