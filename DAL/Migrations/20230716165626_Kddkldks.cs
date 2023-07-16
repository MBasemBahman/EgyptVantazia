using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class Kddkldks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstNotificationJobId",
                table: "TeamGameWeaks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HalfTimeEnded",
                table: "TeamGameWeaks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecondNotificationJobId",
                table: "TeamGameWeaks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThirdNotificationJobId",
                table: "TeamGameWeaks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$C03Yv8OFRkXHcOboxAVJCOOzEyXwq1ioXTq9pjM2UvnGOHxcoNOyK");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstNotificationJobId",
                table: "TeamGameWeaks");

            migrationBuilder.DropColumn(
                name: "HalfTimeEnded",
                table: "TeamGameWeaks");

            migrationBuilder.DropColumn(
                name: "SecondNotificationJobId",
                table: "TeamGameWeaks");

            migrationBuilder.DropColumn(
                name: "ThirdNotificationJobId",
                table: "TeamGameWeaks");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$ut3yxRvrV.oEZapJmMXDnOBkTS.CKPdYIm.zhobAd1qw8mqVqPbzi");
        }
    }
}
