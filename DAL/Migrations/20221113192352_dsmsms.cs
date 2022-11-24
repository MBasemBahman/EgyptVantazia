using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class dsmsms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<string>(
                name: "Order_id",
                table: "AccountSubscriptions",
                type: "nvarchar(max)",
                nullable: true);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$HLIrn9VEtUuyblpMw/hjNOl6Y51KLgF5fGw1iZU2CgrxgilIhAVwi");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "Order_id",
                table: "AccountSubscriptions");

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$VdTgR8JNAcsoKH74/oELJe4KKY9kY3uAb1JbZMayLLNDlR.1EMAIi");
        }
    }
}
