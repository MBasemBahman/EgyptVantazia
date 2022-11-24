using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class dkmd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.InsertData(
            //    table: "TeamPlayerTypes",
            //    columns: new[] { "Id", "LastModifiedBy", "Name" },
            //    values: new object[] { 1, null, "Captian" });

            _ = migrationBuilder.InsertData(
                table: "TeamPlayerTypes",
                columns: new[] { "Id", "LastModifiedBy", "Name" },
                values: new object[] { 2, null, "ViceCaptian" });

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$HcVj022UbsbVXwm4gIMyAenrpC/RXdN9G.uYSgIty3Yg5NObCw2WC");

            //migrationBuilder.InsertData(
            //    table: "TeamPlayerTypeLang",
            //    columns: new[] { "Id", "Fk_Source", "Name" },
            //    values: new object[] { 1, 1, "Captian" });

            _ = migrationBuilder.InsertData(
                table: "TeamPlayerTypeLang",
                columns: new[] { "Id", "Fk_Source", "Name" },
                values: new object[] { 2, 2, "ViceCaptian" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DeleteData(
                table: "TeamPlayerTypeLang",
                keyColumn: "Id",
                keyValue: 1);

            _ = migrationBuilder.DeleteData(
                table: "TeamPlayerTypeLang",
                keyColumn: "Id",
                keyValue: 2);

            _ = migrationBuilder.DeleteData(
                table: "TeamPlayerTypes",
                keyColumn: "Id",
                keyValue: 1);

            _ = migrationBuilder.DeleteData(
                table: "TeamPlayerTypes",
                keyColumn: "Id",
                keyValue: 2);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$qbLGaTmA18xX/SDzVLvz3u4Hk7hJ5prZXtOX.GqCpVjju5opNmwEe");
        }
    }
}
