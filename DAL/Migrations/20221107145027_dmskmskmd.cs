using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class dmskmskmd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropIndex(
                name: "IX_AccountSubscriptions_Fk_Account",
                table: "AccountSubscriptions");

            _ = migrationBuilder.DropColumn(
                name: "EndDate",
                table: "AccountSubscriptions");

            _ = migrationBuilder.DropColumn(
                name: "StartDate",
                table: "AccountSubscriptions");

            _ = migrationBuilder.AddColumn<int>(
                name: "Fk_Season",
                table: "AccountSubscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$OQ65cVR7HJxryE2Duj06TuOSnacyM0e5nXX0WOOdovtjTpE2Ewe7S");

            _ = migrationBuilder.CreateIndex(
                name: "IX_AccountSubscriptions_Fk_Account_Fk_Season_Fk_Subscription",
                table: "AccountSubscriptions",
                columns: new[] { "Fk_Account", "Fk_Season", "Fk_Subscription" },
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_AccountSubscriptions_Fk_Season",
                table: "AccountSubscriptions",
                column: "Fk_Season");

            _ = migrationBuilder.AddForeignKey(
                name: "FK_AccountSubscriptions_Seasons_Fk_Season",
                table: "AccountSubscriptions",
                column: "Fk_Season",
                principalTable: "Seasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_AccountSubscriptions_Seasons_Fk_Season",
                table: "AccountSubscriptions");

            _ = migrationBuilder.DropIndex(
                name: "IX_AccountSubscriptions_Fk_Account_Fk_Season_Fk_Subscription",
                table: "AccountSubscriptions");

            _ = migrationBuilder.DropIndex(
                name: "IX_AccountSubscriptions_Fk_Season",
                table: "AccountSubscriptions");

            _ = migrationBuilder.DropColumn(
                name: "Fk_Season",
                table: "AccountSubscriptions");

            _ = migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "AccountSubscriptions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            _ = migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "AccountSubscriptions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$4xnTUS1iZYWrr1xP8LhW3ONXSO.7Fa8trPIKjtFx3.7Obiov4v18.");

            _ = migrationBuilder.CreateIndex(
                name: "IX_AccountSubscriptions_Fk_Account",
                table: "AccountSubscriptions",
                column: "Fk_Account");
        }
    }
}
