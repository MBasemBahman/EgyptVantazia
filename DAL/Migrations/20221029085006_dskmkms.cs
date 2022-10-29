using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class dskmkms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScoreStates_ScoreTypeLang_ScoreTypeLangId",
                table: "ScoreStates");

            migrationBuilder.DropIndex(
                name: "IX_ScoreStates_ScoreTypeLangId",
                table: "ScoreStates");

            migrationBuilder.DropIndex(
                name: "IX_ScoreStateLang_Fk_Source",
                table: "ScoreStateLang");

            migrationBuilder.DropColumn(
                name: "ScoreTypeLangId",
                table: "ScoreStates");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "PlayerSeasonScoreStates");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "TotalPoints",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "PlayerGameWeakScoreStates");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ScoreStateLang",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getutcdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "PositionByPercent",
                table: "PlayerSeasonScoreStates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PositionByPoints",
                table: "PlayerSeasonScoreStates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PositionByValue",
                table: "PlayerSeasonScoreStates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PositionByPercent",
                table: "PlayerGameWeakScoreStates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PositionByPoints",
                table: "PlayerGameWeakScoreStates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PositionByValue",
                table: "PlayerGameWeakScoreStates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$qbLGaTmA18xX/SDzVLvz3u4Hk7hJ5prZXtOX.GqCpVjju5opNmwEe");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreStateLang_Fk_Source",
                table: "ScoreStateLang",
                column: "Fk_Source",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ScoreStateLang_Fk_Source",
                table: "ScoreStateLang");

            migrationBuilder.DropColumn(
                name: "PositionByPercent",
                table: "PlayerSeasonScoreStates");

            migrationBuilder.DropColumn(
                name: "PositionByPoints",
                table: "PlayerSeasonScoreStates");

            migrationBuilder.DropColumn(
                name: "PositionByValue",
                table: "PlayerSeasonScoreStates");

            migrationBuilder.DropColumn(
                name: "PositionByPercent",
                table: "PlayerGameWeakScoreStates");

            migrationBuilder.DropColumn(
                name: "PositionByPoints",
                table: "PlayerGameWeakScoreStates");

            migrationBuilder.DropColumn(
                name: "PositionByValue",
                table: "PlayerGameWeakScoreStates");

            migrationBuilder.AddColumn<int>(
                name: "ScoreTypeLangId",
                table: "ScoreStates",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ScoreStateLang",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getutcdate()");

            migrationBuilder.AddColumn<double>(
                name: "Position",
                table: "PlayerSeasonScoreStates",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalPoints",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Position",
                table: "PlayerGameWeakScoreStates",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9496));

            migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9508));

            migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9511));

            migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9513));

            migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9515));

            migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9518));

            migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9521));

            migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9523));

            migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9525));

            migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9528));

            migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9530));

            migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9532));

            migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9540));

            migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9543));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$LI6YkwfOom/wy6D5ffk28u6ajscjVfg4SxAZt7qrJm85qZZnWErkW");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreStates_ScoreTypeLangId",
                table: "ScoreStates",
                column: "ScoreTypeLangId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreStateLang_Fk_Source",
                table: "ScoreStateLang",
                column: "Fk_Source");

            migrationBuilder.AddForeignKey(
                name: "FK_ScoreStates_ScoreTypeLang_ScoreTypeLangId",
                table: "ScoreStates",
                column: "ScoreTypeLangId",
                principalTable: "ScoreTypeLang",
                principalColumn: "Id");
        }
    }
}
