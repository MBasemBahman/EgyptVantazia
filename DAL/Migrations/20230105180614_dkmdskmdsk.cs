using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class dkmdskmdsk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$5mY5MYLozd9Q04MJb9yRKOLluMSAreY8aAMYyDUnx254VoAkHPu1C");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$tQnHJJLj3HYRBAvl37CD3u3d.NuAEi11qTXdDr8TZgSPi1GubU8NS");
        }
    }
}
