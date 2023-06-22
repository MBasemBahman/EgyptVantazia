using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class mdkmdkmds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Fk_PromoCode",
                table: "AccountSubscriptions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Marks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PromoCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discount = table.Column<int>(type: "int", nullable: false),
                    MinPrice = table.Column<int>(type: "int", nullable: true),
                    MaxDiscount = table.Column<int>(type: "int", nullable: true),
                    MaxUse = table.Column<int>(type: "int", nullable: true),
                    MaxUsePerUser = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromoCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatisticCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    _365_Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatisticCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MarkLang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    Fk_Source = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarkLang", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarkLang_Marks_Fk_Source",
                        column: x => x.Fk_Source,
                        principalTable: "Marks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerMarks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_Player = table.Column<int>(type: "int", nullable: false),
                    Fk_Mark = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Count = table.Column<int>(type: "int", nullable: true),
                    Used = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerMarks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerMarks_Marks_Fk_Mark",
                        column: x => x.Fk_Mark,
                        principalTable: "Marks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerMarks_Players_Fk_Player",
                        column: x => x.Fk_Player,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PromoCodeLang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    Fk_Source = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromoCodeLang", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromoCodeLang_PromoCodes_Fk_Source",
                        column: x => x.Fk_Source,
                        principalTable: "PromoCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PromoCodeSubscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_PromoCode = table.Column<int>(type: "int", nullable: false),
                    Fk_Subscription = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromoCodeSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromoCodeSubscriptions_PromoCodes_Fk_PromoCode",
                        column: x => x.Fk_PromoCode,
                        principalTable: "PromoCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PromoCodeSubscriptions_Subscriptions_Fk_Subscription",
                        column: x => x.Fk_Subscription,
                        principalTable: "Subscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StatisticCategoryLang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    Fk_Source = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatisticCategoryLang", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatisticCategoryLang_StatisticCategories_Fk_Source",
                        column: x => x.Fk_Source,
                        principalTable: "StatisticCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StatisticScores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    _365_Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fk_StatisticCategory = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatisticScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatisticScores_StatisticCategories_Fk_StatisticCategory",
                        column: x => x.Fk_StatisticCategory,
                        principalTable: "StatisticCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerMarkGameWeakScores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_PlayerMark = table.Column<int>(type: "int", nullable: false),
                    Fk_PlayerGameWeakScore = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerMarkGameWeakScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerMarkGameWeakScores_PlayerGameWeakScores_Fk_PlayerGameWeakScore",
                        column: x => x.Fk_PlayerGameWeakScore,
                        principalTable: "PlayerGameWeakScores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerMarkGameWeakScores_PlayerMarks_Fk_PlayerMark",
                        column: x => x.Fk_PlayerMark,
                        principalTable: "PlayerMarks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlayerMarkGameWeeks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_PlayerMark = table.Column<int>(type: "int", nullable: false),
                    Fk_GameWeak = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerMarkGameWeeks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerMarkGameWeeks_GameWeaks_Fk_GameWeak",
                        column: x => x.Fk_GameWeak,
                        principalTable: "GameWeaks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerMarkGameWeeks_PlayerMarks_Fk_PlayerMark",
                        column: x => x.Fk_PlayerMark,
                        principalTable: "PlayerMarks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerMarkTeamGameWeaks",
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
                    table.PrimaryKey("PK_PlayerMarkTeamGameWeaks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerMarkTeamGameWeaks_PlayerMarks_Fk_PlayerMark",
                        column: x => x.Fk_PlayerMark,
                        principalTable: "PlayerMarks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerMarkTeamGameWeaks_TeamGameWeaks_Fk_TeamGameWeak",
                        column: x => x.Fk_TeamGameWeak,
                        principalTable: "TeamGameWeaks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MatchStatisticScores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_StatisticScore = table.Column<int>(type: "int", nullable: false),
                    Fk_TeamGameWeak = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValuePercentage = table.Column<double>(type: "float", nullable: false),
                    IsCanNotEdit = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchStatisticScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchStatisticScores_StatisticScores_Fk_StatisticScore",
                        column: x => x.Fk_StatisticScore,
                        principalTable: "StatisticScores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MatchStatisticScores_TeamGameWeaks_Fk_TeamGameWeak",
                        column: x => x.Fk_TeamGameWeak,
                        principalTable: "TeamGameWeaks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StatisticScoreLang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    Fk_Source = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatisticScoreLang", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatisticScoreLang_StatisticScores_Fk_Source",
                        column: x => x.Fk_Source,
                        principalTable: "StatisticScores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$sSVeh3FqqihR/nHFtkuQL.AHCuTryWKOOsnmDss.xB.ghjkBadBPO");

            migrationBuilder.CreateIndex(
                name: "IX_AccountSubscriptions_Fk_PromoCode",
                table: "AccountSubscriptions",
                column: "Fk_PromoCode");

            migrationBuilder.CreateIndex(
                name: "IX_MarkLang_Fk_Source",
                table: "MarkLang",
                column: "Fk_Source",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Marks_Name",
                table: "Marks",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MatchStatisticScores_Fk_StatisticScore",
                table: "MatchStatisticScores",
                column: "Fk_StatisticScore");

            migrationBuilder.CreateIndex(
                name: "IX_MatchStatisticScores_Fk_TeamGameWeak",
                table: "MatchStatisticScores",
                column: "Fk_TeamGameWeak");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMarkGameWeakScores_Fk_PlayerGameWeakScore",
                table: "PlayerMarkGameWeakScores",
                column: "Fk_PlayerGameWeakScore");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMarkGameWeakScores_Fk_PlayerMark",
                table: "PlayerMarkGameWeakScores",
                column: "Fk_PlayerMark");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMarkGameWeeks_Fk_GameWeak",
                table: "PlayerMarkGameWeeks",
                column: "Fk_GameWeak");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMarkGameWeeks_Fk_PlayerMark",
                table: "PlayerMarkGameWeeks",
                column: "Fk_PlayerMark");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMarks_Fk_Mark",
                table: "PlayerMarks",
                column: "Fk_Mark");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMarks_Fk_Player",
                table: "PlayerMarks",
                column: "Fk_Player");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMarkTeamGameWeaks_Fk_PlayerMark",
                table: "PlayerMarkTeamGameWeaks",
                column: "Fk_PlayerMark");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMarkTeamGameWeaks_Fk_TeamGameWeak",
                table: "PlayerMarkTeamGameWeaks",
                column: "Fk_TeamGameWeak");

            migrationBuilder.CreateIndex(
                name: "IX_PromoCodeLang_Fk_Source",
                table: "PromoCodeLang",
                column: "Fk_Source",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PromoCodes_Code",
                table: "PromoCodes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PromoCodeSubscriptions_Fk_PromoCode",
                table: "PromoCodeSubscriptions",
                column: "Fk_PromoCode");

            migrationBuilder.CreateIndex(
                name: "IX_PromoCodeSubscriptions_Fk_Subscription",
                table: "PromoCodeSubscriptions",
                column: "Fk_Subscription");

            migrationBuilder.CreateIndex(
                name: "IX_StatisticCategories_Name",
                table: "StatisticCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StatisticCategoryLang_Fk_Source",
                table: "StatisticCategoryLang",
                column: "Fk_Source",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StatisticScoreLang_Fk_Source",
                table: "StatisticScoreLang",
                column: "Fk_Source",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StatisticScores_Fk_StatisticCategory",
                table: "StatisticScores",
                column: "Fk_StatisticCategory");

            migrationBuilder.CreateIndex(
                name: "IX_StatisticScores_Name",
                table: "StatisticScores",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountSubscriptions_PromoCodes_Fk_PromoCode",
                table: "AccountSubscriptions",
                column: "Fk_PromoCode",
                principalTable: "PromoCodes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountSubscriptions_PromoCodes_Fk_PromoCode",
                table: "AccountSubscriptions");

            migrationBuilder.DropTable(
                name: "MarkLang");

            migrationBuilder.DropTable(
                name: "MatchStatisticScores");

            migrationBuilder.DropTable(
                name: "PlayerMarkGameWeakScores");

            migrationBuilder.DropTable(
                name: "PlayerMarkGameWeeks");

            migrationBuilder.DropTable(
                name: "PlayerMarkTeamGameWeaks");

            migrationBuilder.DropTable(
                name: "PromoCodeLang");

            migrationBuilder.DropTable(
                name: "PromoCodeSubscriptions");

            migrationBuilder.DropTable(
                name: "StatisticCategoryLang");

            migrationBuilder.DropTable(
                name: "StatisticScoreLang");

            migrationBuilder.DropTable(
                name: "PlayerMarks");

            migrationBuilder.DropTable(
                name: "PromoCodes");

            migrationBuilder.DropTable(
                name: "StatisticScores");

            migrationBuilder.DropTable(
                name: "Marks");

            migrationBuilder.DropTable(
                name: "StatisticCategories");

            migrationBuilder.DropIndex(
                name: "IX_AccountSubscriptions_Fk_PromoCode",
                table: "AccountSubscriptions");

            migrationBuilder.DropColumn(
                name: "Fk_PromoCode",
                table: "AccountSubscriptions");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$4bv7Acw0iYNsG74i3yy2TeQm1NK6bFT/j2ENacOax62eE8.Sq47Fy");
        }
    }
}
