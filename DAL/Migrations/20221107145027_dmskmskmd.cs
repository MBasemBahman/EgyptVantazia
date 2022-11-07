using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class dmskmskmd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AccountSubscriptions_Fk_Account",
                table: "AccountSubscriptions");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "AccountSubscriptions");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "AccountSubscriptions");

            migrationBuilder.AddColumn<int>(
                name: "Fk_Season",
                table: "AccountSubscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$OQ65cVR7HJxryE2Duj06TuOSnacyM0e5nXX0WOOdovtjTpE2Ewe7S");

            migrationBuilder.CreateIndex(
                name: "IX_AccountSubscriptions_Fk_Account_Fk_Season_Fk_Subscription",
                table: "AccountSubscriptions",
                columns: new[] { "Fk_Account", "Fk_Season", "Fk_Subscription" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountSubscriptions_Fk_Season",
                table: "AccountSubscriptions",
                column: "Fk_Season");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountSubscriptions_Seasons_Fk_Season",
                table: "AccountSubscriptions",
                column: "Fk_Season",
                principalTable: "Seasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountSubscriptions_Seasons_Fk_Season",
                table: "AccountSubscriptions");

            migrationBuilder.DropIndex(
                name: "IX_AccountSubscriptions_Fk_Account_Fk_Season_Fk_Subscription",
                table: "AccountSubscriptions");

            migrationBuilder.DropIndex(
                name: "IX_AccountSubscriptions_Fk_Season",
                table: "AccountSubscriptions");

            migrationBuilder.DropColumn(
                name: "Fk_Season",
                table: "AccountSubscriptions");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "AccountSubscriptions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "AccountSubscriptions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$4xnTUS1iZYWrr1xP8LhW3ONXSO.7Fa8trPIKjtFx3.7Obiov4v18.");

            migrationBuilder.CreateIndex(
                name: "IX_AccountSubscriptions_Fk_Account",
                table: "AccountSubscriptions",
                column: "Fk_Account");
        }
    }
}
