using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class mdsmdmk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AccountSubscriptions_Fk_Account_Fk_Season_Fk_Subscription",
                table: "AccountSubscriptions");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$a218nIorpT3aN3RvQWMrTus1/FiskmgOOgRobxprmX55E.LFhn9dK");

            migrationBuilder.CreateIndex(
                name: "IX_AccountSubscriptions_Fk_Account",
                table: "AccountSubscriptions",
                column: "Fk_Account");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AccountSubscriptions_Fk_Account",
                table: "AccountSubscriptions");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$kjqA/aFrG7VRMqnc51YP0.5of8/wGMvS1FeCxEORxe3Ao0mHPn/JS");

            migrationBuilder.CreateIndex(
                name: "IX_AccountSubscriptions_Fk_Account_Fk_Season_Fk_Subscription",
                table: "AccountSubscriptions",
                columns: new[] { "Fk_Account", "Fk_Season", "Fk_Subscription" },
                unique: true);
        }
    }
}
