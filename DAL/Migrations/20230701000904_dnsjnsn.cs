using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class dnsjnsn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Seasons_Fk_Season",
                table: "Teams");

            migrationBuilder.AlterColumn<int>(
                name: "Fk_Season",
                table: "Teams",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$rdqBDWZYVa58OW2kIpQaUOqx/3ggY0A1GYRUlmREvs0X1tWKbxSgu");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Seasons_Fk_Season",
                table: "Teams",
                column: "Fk_Season",
                principalTable: "Seasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Seasons_Fk_Season",
                table: "Teams");

            migrationBuilder.AlterColumn<int>(
                name: "Fk_Season",
                table: "Teams",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$LEQTrWBEL1u2SMH1YvYadehmvVAJy.5VCUd0FAlLeMivyUxFB0mlC");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Seasons_Fk_Season",
                table: "Teams",
                column: "Fk_Season",
                principalTable: "Seasons",
                principalColumn: "Id");
        }
    }
}
