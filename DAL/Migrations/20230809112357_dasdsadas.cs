using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class dasdsadas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Fk_Season",
                table: "News",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_News_Fk_Season",
                table: "News",
                column: "Fk_Season");

            migrationBuilder.AddForeignKey(
                name: "FK_News_Seasons_Fk_Season",
                table: "News",
                column: "Fk_Season",
                principalTable: "Seasons",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_News_Seasons_Fk_Season",
                table: "News");

            migrationBuilder.DropIndex(
                name: "IX_News_Fk_Season",
                table: "News");

            migrationBuilder.DropColumn(
                name: "Fk_Season",
                table: "News");
        }
    }
}
