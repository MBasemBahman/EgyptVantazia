using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class Mkdkdkj : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<int>(
            //    name: "Order",
            //    table: "Subscriptions",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$Mhd90P4jwurLAkA1AVSUrOEVqOYLuAI4D41YaX.lGzKPjNMK4nR/C");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "Subscriptions");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$J7HiQ/lxFwX6.P.VtD8oPeZn473L0yeQqE1wA5SMol2h9.7C31eAO");
        }
    }
}
