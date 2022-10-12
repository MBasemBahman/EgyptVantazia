using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class mkmdksmdkm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AlterColumn<double>(
                name: "SellPrice",
                table: "PlayerPrices",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            _ = migrationBuilder.AlterColumn<double>(
                name: "BuyPrice",
                table: "PlayerPrices",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$TOhUTTK0HXBZXzlAAS0Cgu2eF1GCJbsC8OgnBkcLiaJQBYfaOkQBi");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AlterColumn<int>(
                name: "SellPrice",
                table: "PlayerPrices",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            _ = migrationBuilder.AlterColumn<int>(
                name: "BuyPrice",
                table: "PlayerPrices",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$NFGeYgTeYOfMBTaIGZonS.MFGSsBtyPFnONi3ITTQB/3BhEqv.bXi");
        }
    }
}
