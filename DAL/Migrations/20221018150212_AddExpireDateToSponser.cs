using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class AddExpireDateToSponser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<DateTime>(
                name: "ExpireDate",
                table: "Sponsors",
                type: "datetime2",
                nullable: true);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$BTPpeRXeZbOHiGspN4vFK.z50Pw0HYuGEsd51z/4leP0lHJo7S2lK");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "ExpireDate",
                table: "Sponsors");

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$0euBoIpNQYm6umDTbADsnuhkTcqdIQZ5J4xaIEtilC3pipeISJca6");
        }
    }
}
