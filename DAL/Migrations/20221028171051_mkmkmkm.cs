using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class mkmkmkm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScoreStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScoreTypeLangId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScoreStates_ScoreTypeLang_ScoreTypeLangId",
                        column: x => x.ScoreTypeLangId,
                        principalTable: "ScoreTypeLang",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PlayerGameWeakScoreStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_Player = table.Column<int>(type: "int", nullable: false),
                    Fk_ScoreState = table.Column<int>(type: "int", nullable: false),
                    Fk_GameWeak = table.Column<int>(type: "int", nullable: false),
                    Points = table.Column<double>(type: "float", nullable: false),
                    Position = table.Column<double>(type: "float", nullable: false),
                    Value = table.Column<double>(type: "float", nullable: false),
                    Percent = table.Column<double>(type: "float", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerGameWeakScoreStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerGameWeakScoreStates_GameWeaks_Fk_GameWeak",
                        column: x => x.Fk_GameWeak,
                        principalTable: "GameWeaks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerGameWeakScoreStates_Players_Fk_Player",
                        column: x => x.Fk_Player,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerGameWeakScoreStates_ScoreStates_Fk_ScoreState",
                        column: x => x.Fk_ScoreState,
                        principalTable: "ScoreStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerSeasonScoreStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_Player = table.Column<int>(type: "int", nullable: false),
                    Fk_ScoreState = table.Column<int>(type: "int", nullable: false),
                    Fk_Season = table.Column<int>(type: "int", nullable: false),
                    Points = table.Column<double>(type: "float", nullable: false),
                    Position = table.Column<double>(type: "float", nullable: false),
                    Value = table.Column<double>(type: "float", nullable: false),
                    Percent = table.Column<double>(type: "float", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerSeasonScoreStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerSeasonScoreStates_Players_Fk_Player",
                        column: x => x.Fk_Player,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerSeasonScoreStates_ScoreStates_Fk_ScoreState",
                        column: x => x.Fk_ScoreState,
                        principalTable: "ScoreStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerSeasonScoreStates_Seasons_Fk_Season",
                        column: x => x.Fk_Season,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScoreStateLang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Fk_Source = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreStateLang", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScoreStateLang_ScoreStates_Fk_Source",
                        column: x => x.Fk_Source,
                        principalTable: "ScoreStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ScoreStates",
                columns: new[] { "Id", "Description", "LastModifiedBy", "Name", "ScoreTypeLangId" },
                values: new object[,]
                {
                    { 1, null, null, "Total", null },
                    { 2, null, null, "CleanSheet", null },
                    { 3, null, null, "Goals", null },
                    { 4, null, null, "Assists", null },
                    { 5, null, null, "GoalkeeperSaves", null },
                    { 6, null, null, "PenaltiesSaved", null },
                    { 7, null, null, "YellowCard", null },
                    { 8, null, null, "RedCard", null },
                    { 9, null, null, "BuyingPrice", null },
                    { 10, null, null, "SellingPrice", null },
                    { 11, null, null, "BuyingCount", null },
                    { 12, null, null, "SellingCount", null },
                    { 13, null, null, "PlayerSelection", null },
                    { 14, null, null, "PlayerCaptain", null }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$LI6YkwfOom/wy6D5ffk28u6ajscjVfg4SxAZt7qrJm85qZZnWErkW");

            migrationBuilder.InsertData(
                table: "ScoreStateLang",
                columns: new[] { "Id", "CreatedAt", "Fk_Source", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9496), 1, "Total" },
                    { 2, new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9508), 2, "CleanSheet" },
                    { 3, new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9511), 3, "Goals" },
                    { 4, new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9513), 4, "Assists" },
                    { 5, new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9515), 5, "GoalkeeperSaves" },
                    { 6, new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9518), 6, "PenaltiesSaved" },
                    { 7, new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9521), 7, "YellowCard" },
                    { 8, new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9523), 8, "RedCard" },
                    { 9, new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9525), 9, "BuyingPrice" },
                    { 10, new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9528), 10, "SellingPrice" },
                    { 11, new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9530), 11, "BuyingCount" },
                    { 12, new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9532), 12, "SellingCount" },
                    { 13, new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9540), 13, "PlayerSelection" },
                    { 14, new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9543), 14, "PlayerCaptain" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerGameWeakScoreStates_Fk_GameWeak",
                table: "PlayerGameWeakScoreStates",
                column: "Fk_GameWeak");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerGameWeakScoreStates_Fk_Player_Fk_GameWeak_Fk_ScoreState",
                table: "PlayerGameWeakScoreStates",
                columns: new[] { "Fk_Player", "Fk_GameWeak", "Fk_ScoreState" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerGameWeakScoreStates_Fk_ScoreState",
                table: "PlayerGameWeakScoreStates",
                column: "Fk_ScoreState");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerSeasonScoreStates_Fk_Player_Fk_Season_Fk_ScoreState",
                table: "PlayerSeasonScoreStates",
                columns: new[] { "Fk_Player", "Fk_Season", "Fk_ScoreState" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerSeasonScoreStates_Fk_ScoreState",
                table: "PlayerSeasonScoreStates",
                column: "Fk_ScoreState");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerSeasonScoreStates_Fk_Season",
                table: "PlayerSeasonScoreStates",
                column: "Fk_Season");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreStateLang_Fk_Source",
                table: "ScoreStateLang",
                column: "Fk_Source");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreStates_Name",
                table: "ScoreStates",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScoreStates_ScoreTypeLangId",
                table: "ScoreStates",
                column: "ScoreTypeLangId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerGameWeakScoreStates");

            migrationBuilder.DropTable(
                name: "PlayerSeasonScoreStates");

            migrationBuilder.DropTable(
                name: "ScoreStateLang");

            migrationBuilder.DropTable(
                name: "ScoreStates");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$pPlGGFPyy9KqUaWCqQOegeBprqBJDKTDylXkyaNMMNLdg7Nd1t8qm");
        }
    }
}
