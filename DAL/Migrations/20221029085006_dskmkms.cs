using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class dskmkms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_ScoreStates_ScoreTypeLang_ScoreTypeLangId",
                table: "ScoreStates");

            _ = migrationBuilder.DropIndex(
                name: "IX_ScoreStates_ScoreTypeLangId",
                table: "ScoreStates");

            _ = migrationBuilder.DropIndex(
                name: "IX_ScoreStateLang_Fk_Source",
                table: "ScoreStateLang");

            _ = migrationBuilder.DropColumn(
                name: "ScoreTypeLangId",
                table: "ScoreStates");

            _ = migrationBuilder.DropColumn(
                name: "Position",
                table: "PlayerSeasonScoreStates");

            _ = migrationBuilder.DropColumn(
                name: "Position",
                table: "Players");

            _ = migrationBuilder.DropColumn(
                name: "TotalPoints",
                table: "Players");

            _ = migrationBuilder.DropColumn(
                name: "Position",
                table: "PlayerGameWeakScoreStates");

            _ = migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ScoreStateLang",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getutcdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            _ = migrationBuilder.AddColumn<int>(
                name: "PositionByPercent",
                table: "PlayerSeasonScoreStates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.AddColumn<int>(
                name: "PositionByPoints",
                table: "PlayerSeasonScoreStates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.AddColumn<int>(
                name: "PositionByValue",
                table: "PlayerSeasonScoreStates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.AddColumn<int>(
                name: "PositionByPercent",
                table: "PlayerGameWeakScoreStates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.AddColumn<int>(
                name: "PositionByPoints",
                table: "PlayerGameWeakScoreStates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.AddColumn<int>(
                name: "PositionByValue",
                table: "PlayerGameWeakScoreStates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            _ = migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            _ = migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            _ = migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            _ = migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            _ = migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            _ = migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            _ = migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            _ = migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            _ = migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            _ = migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            _ = migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            _ = migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            _ = migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$qbLGaTmA18xX/SDzVLvz3u4Hk7hJ5prZXtOX.GqCpVjju5opNmwEe");

            _ = migrationBuilder.CreateIndex(
                name: "IX_ScoreStateLang_Fk_Source",
                table: "ScoreStateLang",
                column: "Fk_Source",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropIndex(
                name: "IX_ScoreStateLang_Fk_Source",
                table: "ScoreStateLang");

            _ = migrationBuilder.DropColumn(
                name: "PositionByPercent",
                table: "PlayerSeasonScoreStates");

            _ = migrationBuilder.DropColumn(
                name: "PositionByPoints",
                table: "PlayerSeasonScoreStates");

            _ = migrationBuilder.DropColumn(
                name: "PositionByValue",
                table: "PlayerSeasonScoreStates");

            _ = migrationBuilder.DropColumn(
                name: "PositionByPercent",
                table: "PlayerGameWeakScoreStates");

            _ = migrationBuilder.DropColumn(
                name: "PositionByPoints",
                table: "PlayerGameWeakScoreStates");

            _ = migrationBuilder.DropColumn(
                name: "PositionByValue",
                table: "PlayerGameWeakScoreStates");

            _ = migrationBuilder.AddColumn<int>(
                name: "ScoreTypeLangId",
                table: "ScoreStates",
                type: "int",
                nullable: true);

            _ = migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ScoreStateLang",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getutcdate()");

            _ = migrationBuilder.AddColumn<double>(
                name: "Position",
                table: "PlayerSeasonScoreStates",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            _ = migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.AddColumn<int>(
                name: "TotalPoints",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.AddColumn<double>(
                name: "Position",
                table: "PlayerGameWeakScoreStates",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            _ = migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9496));

            _ = migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9508));

            _ = migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9511));

            _ = migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9513));

            _ = migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9515));

            _ = migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9518));

            _ = migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9521));

            _ = migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9523));

            _ = migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9525));

            _ = migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9528));

            _ = migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9530));

            _ = migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9532));

            _ = migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9540));

            _ = migrationBuilder.UpdateData(
                table: "ScoreStateLang",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2022, 10, 28, 17, 10, 50, 438, DateTimeKind.Utc).AddTicks(9543));

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$LI6YkwfOom/wy6D5ffk28u6ajscjVfg4SxAZt7qrJm85qZZnWErkW");

            _ = migrationBuilder.CreateIndex(
                name: "IX_ScoreStates_ScoreTypeLangId",
                table: "ScoreStates",
                column: "ScoreTypeLangId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_ScoreStateLang_Fk_Source",
                table: "ScoreStateLang",
                column: "Fk_Source");

            _ = migrationBuilder.AddForeignKey(
                name: "FK_ScoreStates_ScoreTypeLang_ScoreTypeLangId",
                table: "ScoreStates",
                column: "ScoreTypeLangId",
                principalTable: "ScoreTypeLang",
                principalColumn: "Id");
        }
    }
}
