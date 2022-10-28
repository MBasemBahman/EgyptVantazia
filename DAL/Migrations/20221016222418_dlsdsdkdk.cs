using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class dlsdsdkdk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<double>(
                name: "GameTime",
                table: "PlayerGameWeakScores",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$5ynTA8fyPUTkwU7dX2qiD.IEBNkTZZtaKq1zTCsFnsJ1cZnlJ9FI.");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "GameTime",
                table: "PlayerGameWeakScores");

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$1eGvaPpOk/MSPRKhYzPEeeTkbUmQQhhfArMUF.ZSkSiULQpvxiwvW");
        }
    }
}
