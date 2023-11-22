using BaseDB;
using Entities.ServicesModels;
using Entities.TenantModels;
using FantasyLogic;
using IntegrationWith365;
using static Services.KashierServices;

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
            IApiVersioningBuilder apiVersioningBuilder = services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
            }); // Nuget Package: Asp.Versioning.Mvc

            _ = apiVersioningBuilder.AddApiExplorer(options =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VVV";

                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                options.SubstituteApiVersionInUrl = true;
            }); // Nuget Package: Asp.Versioning.Mvc.ApiExplorer
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
                        Limit = 50,
                    }
                };
            });

            _ = services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            _ = services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            _ = services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            _ = services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            _ = services.AddInMemoryRateLimiting();
        }

        public static void ConfigurePaymob(this IServiceCollection services,
          IConfiguration configuration)
        {
            PaymobConfiguration config = configuration.GetSection("PaymobConfiguration")
                                          .Get<PaymobConfiguration>();

            _ = services.AddSingleton(config);
            _ = services.AddScoped<PaymobServices>();
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

        public static void Configurekashier(this IServiceCollection services,
          IConfiguration configuration)
        {
            KashierConfiguration config = configuration.GetSection("kashierConfiguration")
                                               .Get<KashierConfiguration>();

            _ = services.AddSingleton(config);
            _ = services.AddScoped<KashierServices>();
        }
    }
}
