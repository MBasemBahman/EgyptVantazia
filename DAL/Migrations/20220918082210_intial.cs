using Microsoft.EntityFrameworkCore.Migrations;


namespace DAL.Migrations
{
    public partial class intial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.CreateTable(
                name: "AppAbout",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AboutCompany = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AboutApp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TermsAndConditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuestionsAndAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GameRules = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subscriptions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Prizes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WhatsApp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwitterUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FacebookUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstagramUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SnapChatUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_AppAbout", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "Countries",
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
                    _ = table.PrimaryKey("PK_Countries", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "DashboardAccessLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreateAccess = table.Column<bool>(type: "bit", nullable: false),
                    EditAccess = table.Column<bool>(type: "bit", nullable: false),
                    ViewAccess = table.Column<bool>(type: "bit", nullable: false),
                    DeleteAccess = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_DashboardAccessLevels", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "DashboardAdministrationRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_DashboardAdministrationRoles", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "DashboardViews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ViewPath = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_DashboardViews", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StackTrace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Logger = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Logs", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "PlayerPositions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    _365_PositionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StorageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_PlayerPositions", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "PrivateLeagues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UniqueCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_PrivateLeagues", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "ScoreTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    _365_TypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_ScoreTypes", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "Seasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    _365_SeasonId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StorageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Seasons", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "Sponsors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LinkUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StorageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Sponsors", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "TeamPlayerTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_TeamPlayerTypes", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    _365_TeamId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StorageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Teams", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Culture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Users", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "AppAboutLang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AboutCompany = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AboutApp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TermsAndConditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuestionsAndAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GameRules = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subscriptions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Prizes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    Fk_Source = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_AppAboutLang", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_AppAboutLang_AppAbout_Fk_Source",
                        column: x => x.Fk_Source,
                        principalTable: "AppAbout",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "CountryLang",
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
                    _ = table.PrimaryKey("PK_CountryLang", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_CountryLang_Countries_Fk_Source",
                        column: x => x.Fk_Source,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "DashboardAccessLevelLang",
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
                    _ = table.PrimaryKey("PK_DashboardAccessLevelLang", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_DashboardAccessLevelLang_DashboardAccessLevels_Fk_Source",
                        column: x => x.Fk_Source,
                        principalTable: "DashboardAccessLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "DashboardAdministrationRoleLang",
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
                    _ = table.PrimaryKey("PK_DashboardAdministrationRoleLang", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_DashboardAdministrationRoleLang_DashboardAdministrationRoles_Fk_Source",
                        column: x => x.Fk_Source,
                        principalTable: "DashboardAdministrationRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "AdministrationRolePremissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_DashboardAccessLevel = table.Column<int>(type: "int", nullable: false),
                    Fk_DashboardAdministrationRole = table.Column<int>(type: "int", nullable: false),
                    Fk_DashboardView = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_AdministrationRolePremissions", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_AdministrationRolePremissions_DashboardAccessLevels_Fk_DashboardAccessLevel",
                        column: x => x.Fk_DashboardAccessLevel,
                        principalTable: "DashboardAccessLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_AdministrationRolePremissions_DashboardAdministrationRoles_Fk_DashboardAdministrationRole",
                        column: x => x.Fk_DashboardAdministrationRole,
                        principalTable: "DashboardAdministrationRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_AdministrationRolePremissions_DashboardViews_Fk_DashboardView",
                        column: x => x.Fk_DashboardView,
                        principalTable: "DashboardViews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "DashboardViewLang",
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
                    _ = table.PrimaryKey("PK_DashboardViewLang", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_DashboardViewLang_DashboardViews_Fk_Source",
                        column: x => x.Fk_Source,
                        principalTable: "DashboardViews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "PlayerPositionLang",
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
                    _ = table.PrimaryKey("PK_PlayerPositionLang", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_PlayerPositionLang_PlayerPositions_Fk_Source",
                        column: x => x.Fk_Source,
                        principalTable: "PlayerPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "ScoreTypeLang",
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
                    _ = table.PrimaryKey("PK_ScoreTypeLang", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_ScoreTypeLang_ScoreTypes_Fk_Source",
                        column: x => x.Fk_Source,
                        principalTable: "ScoreTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "GameWeaks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    _365_GameWeakId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fk_Season = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_GameWeaks", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_GameWeaks_Seasons_Fk_Season",
                        column: x => x.Fk_Season,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "SeasonLang",
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
                    _ = table.PrimaryKey("PK_SeasonLang", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_SeasonLang_Seasons_Fk_Source",
                        column: x => x.Fk_Source,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "SponsorLang",
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
                    _ = table.PrimaryKey("PK_SponsorLang", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_SponsorLang_Sponsors_Fk_Source",
                        column: x => x.Fk_Source,
                        principalTable: "Sponsors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "SponsorViews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_Sponsor = table.Column<int>(type: "int", nullable: false),
                    AppViewEnum = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_SponsorViews", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_SponsorViews_Sponsors_Fk_Sponsor",
                        column: x => x.Fk_Sponsor,
                        principalTable: "Sponsors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "TeamPlayerTypeLang",
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
                    _ = table.PrimaryKey("PK_TeamPlayerTypeLang", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_TeamPlayerTypeLang_TeamPlayerTypes_Fk_Source",
                        column: x => x.Fk_Source,
                        principalTable: "TeamPlayerTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    _365_PlayerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fk_Team = table.Column<int>(type: "int", nullable: false),
                    Fk_PlayerPosition = table.Column<int>(type: "int", nullable: false),
                    PlayerNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StorageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Players", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_Players_PlayerPositions_Fk_PlayerPosition",
                        column: x => x.Fk_PlayerPosition,
                        principalTable: "PlayerPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_Players_Teams_Fk_Team",
                        column: x => x.Fk_Team,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "Standings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_Season = table.Column<int>(type: "int", nullable: false),
                    Fk_Team = table.Column<int>(type: "int", nullable: false),
                    GamePlayed = table.Column<int>(type: "int", nullable: false),
                    GamesWon = table.Column<int>(type: "int", nullable: false),
                    GamesLost = table.Column<int>(type: "int", nullable: false),
                    GamesEven = table.Column<int>(type: "int", nullable: false),
                    _365_For = table.Column<int>(type: "int", nullable: false),
                    Against = table.Column<int>(type: "int", nullable: false),
                    Ratio = table.Column<int>(type: "int", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Standings", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_Standings_Seasons_Fk_Season",
                        column: x => x.Fk_Season,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_Standings_Teams_Fk_Team",
                        column: x => x.Fk_Team,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "TeamLang",
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
                    _ = table.PrimaryKey("PK_TeamLang", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_TeamLang_Teams_Fk_Source",
                        column: x => x.Fk_Source,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_User = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumberTwo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fk_Country = table.Column<int>(type: "int", nullable: false),
                    Fk_Nationality = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fk_FavouriteTeam = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StorageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Accounts", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_Accounts_Countries_Fk_Country",
                        column: x => x.Fk_Country,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_Accounts_Countries_Fk_Nationality",
                        column: x => x.Fk_Nationality,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    _ = table.ForeignKey(
                        name: "FK_Accounts_Teams_Fk_FavouriteTeam",
                        column: x => x.Fk_FavouriteTeam,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_Accounts_Users_Fk_User",
                        column: x => x.Fk_User,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "DashboardAdministrators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_User = table.Column<int>(type: "int", nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fk_DashboardAdministrationRole = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_DashboardAdministrators", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_DashboardAdministrators_DashboardAdministrationRoles_Fk_DashboardAdministrationRole",
                        column: x => x.Fk_DashboardAdministrationRole,
                        principalTable: "DashboardAdministrationRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_DashboardAdministrators_Users_Fk_User",
                        column: x => x.Fk_User,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_User = table.Column<int>(type: "int", nullable: false),
                    NotificationToken = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DeviceType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppVersion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceVersion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceModel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Devices", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_Devices_Users_Fk_User",
                        column: x => x.Fk_User,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_User = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Revoked = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevokedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReplacedByToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReasonRevoked = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_RefreshTokens_Users_Fk_User",
                        column: x => x.Fk_User,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "Verifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_User = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Verifications", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_Verifications_Users_Fk_User",
                        column: x => x.Fk_User,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "GameWeakLang",
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
                    _ = table.PrimaryKey("PK_GameWeakLang", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_GameWeakLang_GameWeaks_Fk_Source",
                        column: x => x.Fk_Source,
                        principalTable: "GameWeaks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LongDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewsTypeEnum = table.Column<int>(type: "int", nullable: false),
                    Fk_GameWeak = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StorageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_News", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_News_GameWeaks_Fk_GameWeak",
                        column: x => x.Fk_GameWeak,
                        principalTable: "GameWeaks",
                        principalColumn: "Id");
                });

            _ = migrationBuilder.CreateTable(
                name: "TeamGameWeaks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_Home = table.Column<int>(type: "int", nullable: false),
                    Fk_Away = table.Column<int>(type: "int", nullable: false),
                    Fk_GameWeak = table.Column<int>(type: "int", nullable: false),
                    HomeScore = table.Column<int>(type: "int", nullable: false),
                    AwayScore = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    _365_MatchId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    _365_MatchUpId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_TeamGameWeaks", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_TeamGameWeaks_GameWeaks_Fk_GameWeak",
                        column: x => x.Fk_GameWeak,
                        principalTable: "GameWeaks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_TeamGameWeaks_Teams_Fk_Away",
                        column: x => x.Fk_Away,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_TeamGameWeaks_Teams_Fk_Home",
                        column: x => x.Fk_Home,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            _ = migrationBuilder.CreateTable(
                name: "PlayerGameWeaks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_GameWeak = table.Column<int>(type: "int", nullable: false),
                    Fk_Player = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_PlayerGameWeaks", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_PlayerGameWeaks_GameWeaks_Fk_GameWeak",
                        column: x => x.Fk_GameWeak,
                        principalTable: "GameWeaks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_PlayerGameWeaks_Players_Fk_Player",
                        column: x => x.Fk_Player,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "PlayerLang",
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
                    _ = table.PrimaryKey("PK_PlayerLang", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_PlayerLang_Players_Fk_Source",
                        column: x => x.Fk_Source,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "PlayerPrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_Team = table.Column<int>(type: "int", nullable: false),
                    Fk_Player = table.Column<int>(type: "int", nullable: false),
                    BuyPrice = table.Column<int>(type: "int", nullable: false),
                    SellPrice = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_PlayerPrices", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_PlayerPrices_Players_Fk_Player",
                        column: x => x.Fk_Player,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_PlayerPrices_Teams_Fk_Team",
                        column: x => x.Fk_Team,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            _ = migrationBuilder.CreateTable(
                name: "AccountTeams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_Account = table.Column<int>(type: "int", nullable: false),
                    Fk_Season = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalPoints = table.Column<int>(type: "int", nullable: false),
                    TotalMoney = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StorageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_AccountTeams", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_AccountTeams_Accounts_Fk_Account",
                        column: x => x.Fk_Account,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_AccountTeams_Seasons_Fk_Season",
                        column: x => x.Fk_Season,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "PrivateLeagueMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_Account = table.Column<int>(type: "int", nullable: false),
                    Fk_PrivateLeague = table.Column<int>(type: "int", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_PrivateLeagueMembers", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_PrivateLeagueMembers_Accounts_Fk_Account",
                        column: x => x.Fk_Account,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_PrivateLeagueMembers_PrivateLeagues_Fk_PrivateLeague",
                        column: x => x.Fk_PrivateLeague,
                        principalTable: "PrivateLeagues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "NewsAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_News = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileLength = table.Column<double>(type: "float", nullable: false),
                    FileUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StorageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_NewsAttachments", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_NewsAttachments_News_Fk_News",
                        column: x => x.Fk_News,
                        principalTable: "News",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "NewsLang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LongDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    Fk_Source = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_NewsLang", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_NewsLang_News_Fk_Source",
                        column: x => x.Fk_Source,
                        principalTable: "News",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "PlayerGameWeakScores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_PlayerGameWeak = table.Column<int>(type: "int", nullable: false),
                    Fk_ScoreType = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_PlayerGameWeakScores", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_PlayerGameWeakScores_PlayerGameWeaks_Fk_PlayerGameWeak",
                        column: x => x.Fk_PlayerGameWeak,
                        principalTable: "PlayerGameWeaks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_PlayerGameWeakScores_ScoreTypes_Fk_ScoreType",
                        column: x => x.Fk_ScoreType,
                        principalTable: "ScoreTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "AccountTeamGameWeaks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_AccountTeam = table.Column<int>(type: "int", nullable: false),
                    Fk_GameWeak = table.Column<int>(type: "int", nullable: false),
                    BenchBoost = table.Column<bool>(type: "bit", nullable: false),
                    FreeHit = table.Column<bool>(type: "bit", nullable: false),
                    WildCard = table.Column<bool>(type: "bit", nullable: false),
                    TotalPoints = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_AccountTeamGameWeaks", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_AccountTeamGameWeaks_AccountTeams_Fk_AccountTeam",
                        column: x => x.Fk_AccountTeam,
                        principalTable: "AccountTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_AccountTeamGameWeaks_GameWeaks_Fk_GameWeak",
                        column: x => x.Fk_GameWeak,
                        principalTable: "GameWeaks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            _ = migrationBuilder.CreateTable(
                name: "AccountTeamPlayers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_AccountTeam = table.Column<int>(type: "int", nullable: false),
                    Fk_Player = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_AccountTeamPlayers", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_AccountTeamPlayers_AccountTeams_Fk_AccountTeam",
                        column: x => x.Fk_AccountTeam,
                        principalTable: "AccountTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_AccountTeamPlayers_Players_Fk_Player",
                        column: x => x.Fk_Player,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            _ = migrationBuilder.CreateTable(
                name: "PlayerTransfers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_Player = table.Column<int>(type: "int", nullable: false),
                    Fk_AccountTeam = table.Column<int>(type: "int", nullable: false),
                    Fk_GameWeak = table.Column<int>(type: "int", nullable: false),
                    TransferTypeEnum = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<int>(type: "int", nullable: false),
                    IsFree = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_PlayerTransfers", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_PlayerTransfers_AccountTeams_Fk_AccountTeam",
                        column: x => x.Fk_AccountTeam,
                        principalTable: "AccountTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_PlayerTransfers_GameWeaks_Fk_GameWeak",
                        column: x => x.Fk_GameWeak,
                        principalTable: "GameWeaks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    _ = table.ForeignKey(
                        name: "FK_PlayerTransfers_Players_Fk_Player",
                        column: x => x.Fk_Player,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            _ = migrationBuilder.CreateTable(
                name: "AccountTeamPlayerGameWeaks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_AccountTeamPlayer = table.Column<int>(type: "int", nullable: false),
                    Fk_TeamPlayerType = table.Column<int>(type: "int", nullable: false),
                    Fk_GameWeak = table.Column<int>(type: "int", nullable: false),
                    TrippleCaptain = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_AccountTeamPlayerGameWeaks", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_AccountTeamPlayerGameWeaks_AccountTeamPlayers_Fk_AccountTeamPlayer",
                        column: x => x.Fk_AccountTeamPlayer,
                        principalTable: "AccountTeamPlayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_AccountTeamPlayerGameWeaks_GameWeaks_Fk_GameWeak",
                        column: x => x.Fk_GameWeak,
                        principalTable: "GameWeaks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    _ = table.ForeignKey(
                        name: "FK_AccountTeamPlayerGameWeaks_TeamPlayerTypes_Fk_TeamPlayerType",
                        column: x => x.Fk_TeamPlayerType,
                        principalTable: "TeamPlayerTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.InsertData(
                table: "AppAbout",
                columns: new[] { "Id", "AboutApp", "AboutCompany", "EmailAddress", "FacebookUrl", "GameRules", "InstagramUrl", "LastModifiedBy", "Phone", "Prizes", "QuestionsAndAnswer", "SnapChatUrl", "Subscriptions", "TermsAndConditions", "TwitterUrl", "WhatsApp" },
                values: new object[] { 1, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null });

            _ = migrationBuilder.InsertData(
                table: "DashboardAccessLevels",
                columns: new[] { "Id", "CreateAccess", "DeleteAccess", "EditAccess", "Name", "ViewAccess" },
                values: new object[,]
                {
                    { 1, true, true, true, "FullAccess", true },
                    { 2, true, false, true, "DataControl", true },
                    { 3, false, false, false, "Viewer", true }
                });

            _ = migrationBuilder.InsertData(
                table: "DashboardAdministrationRoles",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Developer" });

            _ = migrationBuilder.InsertData(
                table: "DashboardViews",
                columns: new[] { "Id", "Name", "ViewPath" },
                values: new object[,]
                {
                    { 1, "Home", "Home" },
                    { 2, "User", "User" },
                    { 3, "DashboardAdministrator", "DashboardAdministrator" },
                    { 4, "DashboardAccessLevel", "DashboardAccessLevel" },
                    { 5, "DashboardAdministrationRole", "DashboardAdministrationRole" },
                    { 6, "DashboardView", "DashboardView" },
                    { 7, "RefreshToken", "RefreshToken" },
                    { 8, "UserDevice", "UserDevice" },
                    { 9, "Verification", "Verification" }
                });

            _ = migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Culture", "EmailAddress", "LastModifiedBy", "Name", "Password", "PhoneNumber", "UserName" },
                values: new object[] { 1, null, "user@mail.com", null, "Developer", "$2a$11$NFGeYgTeYOfMBTaIGZonS.MFGSsBtyPFnONi3ITTQB/3BhEqv.bXi", null, "Developer" });

            _ = migrationBuilder.InsertData(
                table: "AdministrationRolePremissions",
                columns: new[] { "Id", "Fk_DashboardAccessLevel", "Fk_DashboardAdministrationRole", "Fk_DashboardView" },
                values: new object[,]
                {
                    { 1, 1, 1, 1 },
                    { 2, 1, 1, 2 },
                    { 3, 1, 1, 3 },
                    { 4, 1, 1, 4 },
                    { 5, 1, 1, 5 },
                    { 6, 1, 1, 6 },
                    { 7, 1, 1, 7 },
                    { 8, 1, 1, 8 },
                    { 9, 1, 1, 9 }
                });

            _ = migrationBuilder.InsertData(
                table: "AppAboutLang",
                columns: new[] { "Id", "AboutApp", "AboutCompany", "Fk_Source", "GameRules", "Prizes", "QuestionsAndAnswer", "Subscriptions", "TermsAndConditions" },
                values: new object[] { 1, null, null, 1, null, null, null, null, null });

            _ = migrationBuilder.InsertData(
                table: "DashboardAccessLevelLang",
                columns: new[] { "Id", "Fk_Source", "Name" },
                values: new object[,]
                {
                    { 1, 1, "FullAccess" },
                    { 2, 2, "DataControl" },
                    { 3, 3, "Viewer" }
                });

            _ = migrationBuilder.InsertData(
                table: "DashboardAdministrationRoleLang",
                columns: new[] { "Id", "Fk_Source", "Name" },
                values: new object[] { 1, 1, "Developer" });

            _ = migrationBuilder.InsertData(
                table: "DashboardAdministrators",
                columns: new[] { "Id", "Fk_DashboardAdministrationRole", "Fk_User", "JobTitle", "LastModifiedBy" },
                values: new object[] { 1, 1, 1, "Developer", null });

            _ = migrationBuilder.InsertData(
                table: "DashboardViewLang",
                columns: new[] { "Id", "Fk_Source", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Home" },
                    { 2, 2, "User" },
                    { 3, 3, "DashboardAdministrator" },
                    { 4, 4, "DashboardAccessLevel" },
                    { 5, 5, "DashboardAdministrationRole" },
                    { 6, 6, "DashboardView" },
                    { 7, 7, "RefreshToken" },
                    { 8, 8, "UserDevice" },
                    { 9, 9, "Verification" }
                });

            _ = migrationBuilder.CreateIndex(
                name: "IX_Accounts_Fk_Country",
                table: "Accounts",
                column: "Fk_Country");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Accounts_Fk_FavouriteTeam",
                table: "Accounts",
                column: "Fk_FavouriteTeam");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Accounts_Fk_Nationality",
                table: "Accounts",
                column: "Fk_Nationality");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Accounts_Fk_User",
                table: "Accounts",
                column: "Fk_User",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_AccountTeamGameWeaks_Fk_AccountTeam_Fk_GameWeak",
                table: "AccountTeamGameWeaks",
                columns: new[] { "Fk_AccountTeam", "Fk_GameWeak" },
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_AccountTeamGameWeaks_Fk_GameWeak",
                table: "AccountTeamGameWeaks",
                column: "Fk_GameWeak");

            _ = migrationBuilder.CreateIndex(
                name: "IX_AccountTeamPlayerGameWeaks_Fk_AccountTeamPlayer",
                table: "AccountTeamPlayerGameWeaks",
                column: "Fk_AccountTeamPlayer");

            _ = migrationBuilder.CreateIndex(
                name: "IX_AccountTeamPlayerGameWeaks_Fk_GameWeak_Fk_TeamPlayerType_Fk_AccountTeamPlayer",
                table: "AccountTeamPlayerGameWeaks",
                columns: new[] { "Fk_GameWeak", "Fk_TeamPlayerType", "Fk_AccountTeamPlayer" },
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_AccountTeamPlayerGameWeaks_Fk_TeamPlayerType",
                table: "AccountTeamPlayerGameWeaks",
                column: "Fk_TeamPlayerType");

            _ = migrationBuilder.CreateIndex(
                name: "IX_AccountTeamPlayers_Fk_AccountTeam",
                table: "AccountTeamPlayers",
                column: "Fk_AccountTeam");

            _ = migrationBuilder.CreateIndex(
                name: "IX_AccountTeamPlayers_Fk_Player_Fk_AccountTeam",
                table: "AccountTeamPlayers",
                columns: new[] { "Fk_Player", "Fk_AccountTeam" },
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_AccountTeams_Fk_Account_Fk_Season",
                table: "AccountTeams",
                columns: new[] { "Fk_Account", "Fk_Season" },
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_AccountTeams_Fk_Season",
                table: "AccountTeams",
                column: "Fk_Season");

            _ = migrationBuilder.CreateIndex(
                name: "IX_AdministrationRolePremissions_Fk_DashboardAccessLevel",
                table: "AdministrationRolePremissions",
                column: "Fk_DashboardAccessLevel");

            _ = migrationBuilder.CreateIndex(
                name: "IX_AdministrationRolePremissions_Fk_DashboardAdministrationRole_Fk_DashboardView",
                table: "AdministrationRolePremissions",
                columns: new[] { "Fk_DashboardAdministrationRole", "Fk_DashboardView" },
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_AdministrationRolePremissions_Fk_DashboardView",
                table: "AdministrationRolePremissions",
                column: "Fk_DashboardView");

            _ = migrationBuilder.CreateIndex(
                name: "IX_AppAboutLang_Fk_Source",
                table: "AppAboutLang",
                column: "Fk_Source",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_Countries_Name",
                table: "Countries",
                column: "Name",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_CountryLang_Fk_Source",
                table: "CountryLang",
                column: "Fk_Source",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_DashboardAccessLevelLang_Fk_Source",
                table: "DashboardAccessLevelLang",
                column: "Fk_Source",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_DashboardAccessLevels_Name",
                table: "DashboardAccessLevels",
                column: "Name",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_DashboardAdministrationRoleLang_Fk_Source",
                table: "DashboardAdministrationRoleLang",
                column: "Fk_Source",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_DashboardAdministrationRoles_Name",
                table: "DashboardAdministrationRoles",
                column: "Name",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_DashboardAdministrators_Fk_DashboardAdministrationRole",
                table: "DashboardAdministrators",
                column: "Fk_DashboardAdministrationRole");

            _ = migrationBuilder.CreateIndex(
                name: "IX_DashboardAdministrators_Fk_User",
                table: "DashboardAdministrators",
                column: "Fk_User",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_DashboardViewLang_Fk_Source",
                table: "DashboardViewLang",
                column: "Fk_Source",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_DashboardViews_Name",
                table: "DashboardViews",
                column: "Name",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_DashboardViews_ViewPath",
                table: "DashboardViews",
                column: "ViewPath",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_Devices_Fk_User",
                table: "Devices",
                column: "Fk_User");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Devices_NotificationToken",
                table: "Devices",
                column: "NotificationToken",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_GameWeakLang_Fk_Source",
                table: "GameWeakLang",
                column: "Fk_Source",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_GameWeaks_Fk_Season_Name",
                table: "GameWeaks",
                columns: new[] { "Fk_Season", "Name" },
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_News_Fk_GameWeak",
                table: "News",
                column: "Fk_GameWeak");

            _ = migrationBuilder.CreateIndex(
                name: "IX_NewsAttachments_Fk_News",
                table: "NewsAttachments",
                column: "Fk_News");

            _ = migrationBuilder.CreateIndex(
                name: "IX_NewsLang_Fk_Source",
                table: "NewsLang",
                column: "Fk_Source",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_PlayerGameWeaks_Fk_GameWeak_Fk_Player",
                table: "PlayerGameWeaks",
                columns: new[] { "Fk_GameWeak", "Fk_Player" },
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_PlayerGameWeaks_Fk_Player",
                table: "PlayerGameWeaks",
                column: "Fk_Player");

            _ = migrationBuilder.CreateIndex(
                name: "IX_PlayerGameWeakScores_Fk_PlayerGameWeak_Fk_ScoreType",
                table: "PlayerGameWeakScores",
                columns: new[] { "Fk_PlayerGameWeak", "Fk_ScoreType" },
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_PlayerGameWeakScores_Fk_ScoreType",
                table: "PlayerGameWeakScores",
                column: "Fk_ScoreType");

            _ = migrationBuilder.CreateIndex(
                name: "IX_PlayerLang_Fk_Source",
                table: "PlayerLang",
                column: "Fk_Source",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_PlayerPositionLang_Fk_Source",
                table: "PlayerPositionLang",
                column: "Fk_Source",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_PlayerPositions_Name",
                table: "PlayerPositions",
                column: "Name",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_PlayerPrices_Fk_Player",
                table: "PlayerPrices",
                column: "Fk_Player");

            _ = migrationBuilder.CreateIndex(
                name: "IX_PlayerPrices_Fk_Team",
                table: "PlayerPrices",
                column: "Fk_Team");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Players_Fk_PlayerPosition",
                table: "Players",
                column: "Fk_PlayerPosition");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Players_Fk_Team",
                table: "Players",
                column: "Fk_Team");

            _ = migrationBuilder.CreateIndex(
                name: "IX_PlayerTransfers_Fk_AccountTeam",
                table: "PlayerTransfers",
                column: "Fk_AccountTeam");

            _ = migrationBuilder.CreateIndex(
                name: "IX_PlayerTransfers_Fk_GameWeak",
                table: "PlayerTransfers",
                column: "Fk_GameWeak");

            _ = migrationBuilder.CreateIndex(
                name: "IX_PlayerTransfers_Fk_Player",
                table: "PlayerTransfers",
                column: "Fk_Player");

            _ = migrationBuilder.CreateIndex(
                name: "IX_PrivateLeagueMembers_Fk_Account_Fk_PrivateLeague",
                table: "PrivateLeagueMembers",
                columns: new[] { "Fk_Account", "Fk_PrivateLeague" },
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_PrivateLeagueMembers_Fk_PrivateLeague",
                table: "PrivateLeagueMembers",
                column: "Fk_PrivateLeague");

            _ = migrationBuilder.CreateIndex(
                name: "IX_PrivateLeagues_UniqueCode",
                table: "PrivateLeagues",
                column: "UniqueCode",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Fk_User",
                table: "RefreshTokens",
                column: "Fk_User");

            _ = migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Token",
                table: "RefreshTokens",
                column: "Token",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_ScoreTypeLang_Fk_Source",
                table: "ScoreTypeLang",
                column: "Fk_Source",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_ScoreTypes_Name",
                table: "ScoreTypes",
                column: "Name",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_SeasonLang_Fk_Source",
                table: "SeasonLang",
                column: "Fk_Source",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_Seasons_Name",
                table: "Seasons",
                column: "Name",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_SponsorLang_Fk_Source",
                table: "SponsorLang",
                column: "Fk_Source",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_Sponsors_Name",
                table: "Sponsors",
                column: "Name",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_SponsorViews_Fk_Sponsor_AppViewEnum",
                table: "SponsorViews",
                columns: new[] { "Fk_Sponsor", "AppViewEnum" },
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_Standings_Fk_Season_Fk_Team",
                table: "Standings",
                columns: new[] { "Fk_Season", "Fk_Team" },
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_Standings_Fk_Team",
                table: "Standings",
                column: "Fk_Team");

            _ = migrationBuilder.CreateIndex(
                name: "IX_TeamGameWeaks_Fk_Away_Fk_Home_Fk_GameWeak",
                table: "TeamGameWeaks",
                columns: new[] { "Fk_Away", "Fk_Home", "Fk_GameWeak" },
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_TeamGameWeaks_Fk_GameWeak",
                table: "TeamGameWeaks",
                column: "Fk_GameWeak");

            _ = migrationBuilder.CreateIndex(
                name: "IX_TeamGameWeaks_Fk_Home",
                table: "TeamGameWeaks",
                column: "Fk_Home");

            _ = migrationBuilder.CreateIndex(
                name: "IX_TeamLang_Fk_Source",
                table: "TeamLang",
                column: "Fk_Source",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_TeamPlayerTypeLang_Fk_Source",
                table: "TeamPlayerTypeLang",
                column: "Fk_Source",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_TeamPlayerTypes_Name",
                table: "TeamPlayerTypes",
                column: "Name",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_Teams_Name",
                table: "Teams",
                column: "Name",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_Verifications_Code",
                table: "Verifications",
                column: "Code",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_Verifications_Fk_User",
                table: "Verifications",
                column: "Fk_User");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(
                name: "AccountTeamGameWeaks");

            _ = migrationBuilder.DropTable(
                name: "AccountTeamPlayerGameWeaks");

            _ = migrationBuilder.DropTable(
                name: "AdministrationRolePremissions");

            _ = migrationBuilder.DropTable(
                name: "AppAboutLang");

            _ = migrationBuilder.DropTable(
                name: "CountryLang");

            _ = migrationBuilder.DropTable(
                name: "DashboardAccessLevelLang");

            _ = migrationBuilder.DropTable(
                name: "DashboardAdministrationRoleLang");

            _ = migrationBuilder.DropTable(
                name: "DashboardAdministrators");

            _ = migrationBuilder.DropTable(
                name: "DashboardViewLang");

            _ = migrationBuilder.DropTable(
                name: "Devices");

            _ = migrationBuilder.DropTable(
                name: "GameWeakLang");

            _ = migrationBuilder.DropTable(
                name: "Logs");

            _ = migrationBuilder.DropTable(
                name: "NewsAttachments");

            _ = migrationBuilder.DropTable(
                name: "NewsLang");

            _ = migrationBuilder.DropTable(
                name: "PlayerGameWeakScores");

            _ = migrationBuilder.DropTable(
                name: "PlayerLang");

            _ = migrationBuilder.DropTable(
                name: "PlayerPositionLang");

            _ = migrationBuilder.DropTable(
                name: "PlayerPrices");

            _ = migrationBuilder.DropTable(
                name: "PlayerTransfers");

            _ = migrationBuilder.DropTable(
                name: "PrivateLeagueMembers");

            _ = migrationBuilder.DropTable(
                name: "RefreshTokens");

            _ = migrationBuilder.DropTable(
                name: "ScoreTypeLang");

            _ = migrationBuilder.DropTable(
                name: "SeasonLang");

            _ = migrationBuilder.DropTable(
                name: "SponsorLang");

            _ = migrationBuilder.DropTable(
                name: "SponsorViews");

            _ = migrationBuilder.DropTable(
                name: "Standings");

            _ = migrationBuilder.DropTable(
                name: "TeamGameWeaks");

            _ = migrationBuilder.DropTable(
                name: "TeamLang");

            _ = migrationBuilder.DropTable(
                name: "TeamPlayerTypeLang");

            _ = migrationBuilder.DropTable(
                name: "Verifications");

            _ = migrationBuilder.DropTable(
                name: "AccountTeamPlayers");

            _ = migrationBuilder.DropTable(
                name: "AppAbout");

            _ = migrationBuilder.DropTable(
                name: "DashboardAccessLevels");

            _ = migrationBuilder.DropTable(
                name: "DashboardAdministrationRoles");

            _ = migrationBuilder.DropTable(
                name: "DashboardViews");

            _ = migrationBuilder.DropTable(
                name: "News");

            _ = migrationBuilder.DropTable(
                name: "PlayerGameWeaks");

            _ = migrationBuilder.DropTable(
                name: "PrivateLeagues");

            _ = migrationBuilder.DropTable(
                name: "ScoreTypes");

            _ = migrationBuilder.DropTable(
                name: "Sponsors");

            _ = migrationBuilder.DropTable(
                name: "TeamPlayerTypes");

            _ = migrationBuilder.DropTable(
                name: "AccountTeams");

            _ = migrationBuilder.DropTable(
                name: "GameWeaks");

            _ = migrationBuilder.DropTable(
                name: "Players");

            _ = migrationBuilder.DropTable(
                name: "Accounts");

            _ = migrationBuilder.DropTable(
                name: "Seasons");

            _ = migrationBuilder.DropTable(
                name: "PlayerPositions");

            _ = migrationBuilder.DropTable(
                name: "Countries");

            _ = migrationBuilder.DropTable(
                name: "Teams");

            _ = migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
