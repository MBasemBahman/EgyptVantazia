using BaseDB;
using FantasyLogic;
using Hangfire;
using Hangfire.SqlServer;
using IntegrationWith365;

namespace FantasyLogicMicroservices.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {

            _ = services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    _ = builder.SetIsOriginAllowed(a => true)
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials()
                           .WithExposedHeaders(HeadersConstants.Status,
                                               HeadersConstants.Pagination,
                                               HeadersConstants.Authorization,
                                               HeadersConstants.Expires,
                                               HeadersConstants.SetRefresh);
                });
            });
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            _ = services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        public static void ConfigureSingletonService(this IServiceCollection services)
        {
            _ = services.AddSingleton<IJwtUtils, JwtUtils>();
        }

        public static void ConfigureScopedService(this IServiceCollection services)
        {
            _ = services.AddScoped<IAuthenticationManager, AuthenticationManager>();

            _ = services.AddScoped<ServicesHttpClient>();
            _ = services.AddScoped<_365Services>();
            _ = services.AddScoped<FantasyUnitOfWork>();
        }

        public static void ConfigureSqlContext(this IServiceCollection services,
            IConfiguration configuration,
            TenantConfig config)
        {
            if (config.Tenant == TenantEnvironments.Development)
            {
                _ = services.AddDbContext<BaseContext, DBContext>(options => options.UseSqlServer(configuration.GetConnectionString("sqlConnection"),
                                                                options => options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));
            }
        }

        public static void ConfigureRepositoryManager(this IServiceCollection services)
        {
            _ = services.AddScoped<RepositoryManager>();
            _ = services.AddScoped<UnitOfWork>();
        }

        public static void ConfigureVersioning(this IServiceCollection services)
        {
            _ = services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            _ = services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });
        }

        public static void ConfigureResponseCaching(this IServiceCollection services)
        {
            _ = services.AddResponseCaching();
        }

        public static void ConfigureSwagger(this IServiceCollection services, TenantConfig config)
        {
            _ = services.AddSwaggerGen(c =>
            {
                c.MapType<TimeSpan>(() => new OpenApiSchema
                {
                    Type = "string",
                    Example = new OpenApiString("hh:mm:ss")
                });

                c.MapType<DateTime>(() => new OpenApiSchema
                {
                    Type = "string",
                    Example = new OpenApiString("yyyy-MM-ddThh:mm:ss")
                });

                c.SwaggerDoc("Handling", new OpenApiInfo { Title = "Handling" });
                c.SwaggerDoc("Season", new OpenApiInfo { Title = "Season" });
                c.SwaggerDoc("Standings", new OpenApiInfo { Title = "Standings" });
                c.SwaggerDoc("Team", new OpenApiInfo { Title = "Team" });
                c.SwaggerDoc("Games", new OpenApiInfo { Title = "Games" });
                c.SwaggerDoc("AccountTeam", new OpenApiInfo { Title = "AccountTeam" });

                c.OperationFilter<DocsFilter>();
                c.SchemaFilter<SwaggerSkipPropertyFilter>();

                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public static void ConfigureLocalization(this IServiceCollection services)
        {
            _ = services.AddLocalization(options => options.ResourcesPath = "Resources");

            _ = services.Configure<RequestLocalizationOptions>(options =>
            {
                CultureInfo[] supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("ar")
                };
                options.DefaultRequestCulture = new RequestCulture(culture: "en", uiCulture: "en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            _ = services.AddSingleton<ILocalizationManager, LocalizationManager>();
        }

        public static void ConfigureFirebase(this IServiceCollection services,
            string appSettingsFile)
        {
            JToken jAppSettings = JToken.Parse(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, appSettingsFile)));

            _ = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromJson(jAppSettings["GoogleCredential"].ToString())
            });

            _ = services.AddScoped<IFirebaseNotificationManager, FirebaseNotificationManager>();
        }

        public static void ConfigureHangfire(this IServiceCollection services,
           IConfiguration configuration)
        {
            _ = services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromHours(1),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true,
                    JobExpirationCheckInterval = TimeSpan.Zero,
                }));
            // Add the processing server as IHostedService
            _ = services.AddHangfireServer(a =>
            {
                a.WorkerCount = Environment.ProcessorCount * 50;
                a.ServerName = "default";
            });
        }
    }
}
