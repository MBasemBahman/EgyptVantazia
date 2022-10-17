using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class dlsdsdkdk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "GameTime",
                table: "PlayerGameWeakScores",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$5ynTA8fyPUTkwU7dX2qiD.IEBNkTZZtaKq1zTCsFnsJ1cZnlJ9FI.");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameTime",
                table: "PlayerGameWeakScores");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$1eGvaPpOk/MSPRKhYzPEeeTkbUmQQhhfArMUF.ZSkSiULQpvxiwvW");
        }
    }
}
