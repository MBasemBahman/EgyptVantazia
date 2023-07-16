using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class Mkjjkj : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FormationPositions_Name",
                table: "FormationPositions");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "FormationPositions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$ut3yxRvrV.oEZapJmMXDnOBkTS.CKPdYIm.zhobAd1qw8mqVqPbzi");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "FormationPositions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$ePzgLeAbXR6508p3HzCbTuOD2cWPzVM5QjP8btv09qWXa8337QomO");

            migrationBuilder.CreateIndex(
                name: "IX_FormationPositions_Name",
                table: "FormationPositions",
                column: "Name",
                unique: true);
        }
    }
}
