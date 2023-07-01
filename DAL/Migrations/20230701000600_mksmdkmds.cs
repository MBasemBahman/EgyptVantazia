using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class mksmdkmds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Teams_Name",
                table: "Teams");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Teams",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Fk_Season",
                table: "Teams",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$LEQTrWBEL1u2SMH1YvYadehmvVAJy.5VCUd0FAlLeMivyUxFB0mlC");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_Fk_Season",
                table: "Teams",
                column: "Fk_Season");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Seasons_Fk_Season",
                table: "Teams",
                column: "Fk_Season",
                principalTable: "Seasons",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Seasons_Fk_Season",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_Fk_Season",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Fk_Season",
                table: "Teams");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Teams",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$7s/v2fna6aHKGffoVB03WuHV2f3etGJMhecgaSTz7/iTWEy/ziebm");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_Name",
                table: "Teams",
                column: "Name",
                unique: true);
        }
    }
}
