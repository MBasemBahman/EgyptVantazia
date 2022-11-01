using Entities.ServicesModels;
using Entities.TenantModels;
using FantasyLogic;
using Hangfire;
using Hangfire.SqlServer;
using IntegrationWith365;
using Live;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
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
                _ = services.AddDbContext<DbContext, DBContext>(options => options.UseSqlServer(configuration.GetConnectionString("sqlConnection"),
                                                                options => options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));
            }
            else if (config.Tenant == TenantEnvironments.Live)
            {
                _ = services.AddDbContext<DbContext, LiveDBContext>(options => options.UseSqlServer(configuration.GetConnectionString("sqlConnection"),
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

                foreach (SwaggerModel page in config.SwaggerPages)
                {
                    c.SwaggerDoc(page.Name, new OpenApiInfo { Title = page.Title });
                }

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

        public static void ConfigureEmailSender(this IServiceCollection services,
            IConfiguration configuration)
        {
            EmailConfiguration emailConfig = configuration.GetSection("EmailConfiguration")
                                           .Get<EmailConfiguration>();
            _ = services.AddSingleton(emailConfig);
            _ = services.AddScoped<IEmailSender, EmailSender>();
        }

        public static void ConfigureRequestsLimit(this IServiceCollection services)
        {
            // needed to store rate limit counters and ip rules
            _ = services.AddMemoryCache();

            _ = services.Configure<IpRateLimitOptions>(options =>
            {
                options.EnableEndpointRateLimiting = true;
                options.StackBlockedRequests = false;
                options.HttpStatusCode = 429;
                options.RealIpHeader = "X-Real-IP";
                options.ClientIdHeader = "X-ClientId";
                options.GeneralRules = new List<RateLimitRule>
                {
                    new RateLimitRule
                    {
                        Endpoint = "*",
                        Period = "1s",
                        Limit = 100,
                    }
                };
            });

            _ = services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            _ = services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            _ = services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            _ = services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            _ = services.AddInMemoryRateLimiting();
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
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    UsePageLocksOnDequeue = true,
                    DisableGlobalLocks = true
                }));
            // Add the processing server as IHostedService
            _ = services.AddHangfireServer();

            //_ = services.AddScoped<HangfireManager>();
        }

        public static void ConfigurePaymob(this IServiceCollection services,
          IConfiguration configuration)
        {
            PaymobConfiguration config = configuration.GetSection("PaymobConfiguration")
                                          .Get<PaymobConfiguration>();

            _ = services.AddSingleton(config);
            _ = services.AddScoped<PaymobServices>();
        }
    }
}
