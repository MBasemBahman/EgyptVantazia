using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class NewPlayerInfos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Birthdate",
                table: "Players",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Fk_FormationPosition",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FormationPositions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    _365_PositionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StorageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormationPositions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormationPositionLang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    Fk_Source = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormationPositionLang", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormationPositionLang_FormationPositions_Fk_Source",
                        column: x => x.Fk_Source,
                        principalTable: "FormationPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$pQPeho2ck25LhGBRvdxqoeYSLXdv0rH/b.2OD71Ph/VcQBELNIjUO");

            migrationBuilder.CreateIndex(
                name: "IX_Players_Fk_FormationPosition",
                table: "Players",
                column: "Fk_FormationPosition");

            migrationBuilder.CreateIndex(
                name: "IX_FormationPositionLang_Fk_Source",
                table: "FormationPositionLang",
                column: "Fk_Source",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FormationPositions_Name",
                table: "FormationPositions",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_FormationPositions_Fk_FormationPosition",
                table: "Players",
                column: "Fk_FormationPosition",
                principalTable: "FormationPositions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_FormationPositions_Fk_FormationPosition",
                table: "Players");

            migrationBuilder.DropTable(
                name: "FormationPositionLang");

            migrationBuilder.DropTable(
                name: "FormationPositions");

            migrationBuilder.DropIndex(
                name: "IX_Players_Fk_FormationPosition",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Birthdate",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Fk_FormationPosition",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "Players");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$uIrdHe9OZAVIbh.IMLXn2uWZIjJXCQKUyT3gKnwQgwVfdZ7p74yoe");
        }
    }
}
