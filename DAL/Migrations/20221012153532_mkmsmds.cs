using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class mkmsmds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<double>(
                name: "Ranking",
                table: "PlayerGameWeaks",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$8yUWBK0OY9raQo.XlfSNMOPv9LZqG5rKSgPGjCTSPsrSdupwfFTyS");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "Ranking",
                table: "PlayerGameWeaks");

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$p6ldbBMB3Qfd7qpKsJuhc.kce02MjEyJRl1aK0wiVyhcYMOd.2/Mu");
        }
    }
}
