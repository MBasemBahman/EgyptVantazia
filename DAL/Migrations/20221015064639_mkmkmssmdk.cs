using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class mkmkmssmdk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_PlayerGameWeaks_GameWeaks_Fk_GameWeak",
                table: "PlayerGameWeaks");

            _ = migrationBuilder.RenameColumn(
                name: "Fk_GameWeak",
                table: "PlayerGameWeaks",
                newName: "Fk_TeamGameWeak");

            _ = migrationBuilder.RenameIndex(
                name: "IX_PlayerGameWeaks_Fk_GameWeak_Fk_Player",
                table: "PlayerGameWeaks",
                newName: "IX_PlayerGameWeaks_Fk_TeamGameWeak_Fk_Player");

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$0UaEUxl.bY4I.JDibGRd3.hMOrX9YyLw5YUzEcvGASgJOZQhWRR82");

            _ = migrationBuilder.AddForeignKey(
                name: "FK_PlayerGameWeaks_TeamGameWeaks_Fk_TeamGameWeak",
                table: "PlayerGameWeaks",
                column: "Fk_TeamGameWeak",
                principalTable: "TeamGameWeaks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_PlayerGameWeaks_TeamGameWeaks_Fk_TeamGameWeak",
                table: "PlayerGameWeaks");

            _ = migrationBuilder.RenameColumn(
                name: "Fk_TeamGameWeak",
                table: "PlayerGameWeaks",
                newName: "Fk_GameWeak");

            _ = migrationBuilder.RenameIndex(
                name: "IX_PlayerGameWeaks_Fk_TeamGameWeak_Fk_Player",
                table: "PlayerGameWeaks",
                newName: "IX_PlayerGameWeaks_Fk_GameWeak_Fk_Player");

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$PkfDI4VM/7jFuKR3OIlBzuN/qDWaBtePqpWadqWD2hA1LzmG.PJWa");

            _ = migrationBuilder.AddForeignKey(
                name: "FK_PlayerGameWeaks_GameWeaks_Fk_GameWeak",
                table: "PlayerGameWeaks",
                column: "Fk_GameWeak",
                principalTable: "GameWeaks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
