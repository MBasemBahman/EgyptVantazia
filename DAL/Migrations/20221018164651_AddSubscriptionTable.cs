using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class AddSubscriptionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<string>(
                name: "RefCode",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: true);

            _ = migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StorageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Subscriptions", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "AccountSubscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_Account = table.Column<int>(type: "int", nullable: false),
                    Fk_Subscription = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsAction = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_AccountSubscriptions", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_AccountSubscriptions_Accounts_Fk_Account",
                        column: x => x.Fk_Account,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_AccountSubscriptions_Subscriptions_Fk_Subscription",
                        column: x => x.Fk_Subscription,
                        principalTable: "Subscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "SubscriptionLang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    Fk_Source = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_SubscriptionLang", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_SubscriptionLang_Subscriptions_Fk_Source",
                        column: x => x.Fk_Source,
                        principalTable: "Subscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$f7fKVj7ezshNjpCaTNkZcOVMcEFqR55UjAOS08nikR3t0g1tAjjpa");


            _ = migrationBuilder.CreateIndex(
                name: "IX_AccountSubscriptions_Fk_Account",
                table: "AccountSubscriptions",
                column: "Fk_Account");

            _ = migrationBuilder.CreateIndex(
                name: "IX_AccountSubscriptions_Fk_Subscription",
                table: "AccountSubscriptions",
                column: "Fk_Subscription");

            _ = migrationBuilder.CreateIndex(
                name: "IX_SubscriptionLang_Fk_Source",
                table: "SubscriptionLang",
                column: "Fk_Source",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_Name",
                table: "Subscriptions",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(
                name: "AccountSubscriptions");

            _ = migrationBuilder.DropTable(
                name: "SubscriptionLang");

            _ = migrationBuilder.DropTable(
                name: "Subscriptions");

            _ = migrationBuilder.DropColumn(
                name: "RefCode",
                table: "Accounts");

            _ = migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$BTPpeRXeZbOHiGspN4vFK.z50Pw0HYuGEsd51z/4leP0lHJo7S2lK");
        }
    }
}
