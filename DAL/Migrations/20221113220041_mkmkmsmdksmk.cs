using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class mkmkmsmdksmk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Teams",
                type: "bit",
                nullable: false,
                defaultValue: false);

            _ = migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Players",
                type: "bit",
                nullable: false,
                defaultValue: false);

            _ = migrationBuilder.AddColumn<DateTime>(
                name: "Deadline",
                table: "GameWeaks",
                type: "datetime2",
                nullable: true);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$ApDIMnqtUCCFNqAabtaL0ukcgFx0FKfWOguWadNPh2M2WPcuAy.wa");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Teams");

            _ = migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Players");

            _ = migrationBuilder.DropColumn(
                name: "Deadline",
                table: "GameWeaks");

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$HLIrn9VEtUuyblpMw/hjNOl6Y51KLgF5fGw1iZU2CgrxgilIhAVwi");
        }
    }
}
