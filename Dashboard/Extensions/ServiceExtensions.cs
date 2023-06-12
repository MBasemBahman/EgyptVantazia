using BaseDB;
using Newtonsoft.Json.Linq;

namespace Dashboard.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            _ = services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    _ = builder.SetIsOriginAllowed(origin => true)
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials()
                           .WithExposedHeaders(HeadersConstants.AuthorizationToken,
                                               HeadersConstants.AppKey,
                                               HeadersConstants.SetRefresh);
                });
            });
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            _ = services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        public static void ConfigureScopedService(this IServiceCollection services)
        {
            _ = services.AddScoped<IJwtUtils, JwtUtils>();
            _ = services.AddScoped<IAuthenticationManager, AuthenticationManager>();
            _ = services.AddScoped<ServicesHttpClient, ServicesHttpClient>();
            _ = services.AddScoped<UpdateResultsUtils, UpdateResultsUtils>();
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
            _ = services.AddScoped<RepositoryManager, RepositoryManager>();
            _ = services.AddScoped<UnitOfWork, UnitOfWork>();
        }

        public static void ConfigureResponseCaching(this IServiceCollection services)
        {
            _ = services.AddResponseCaching();
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
                options.DefaultRequestCulture = new RequestCulture(culture: "en", uiCulture: "ar");
                //options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            _ = services.AddSingleton<ILocalizationManager, LocalizationManager>();
        }

        public static void ConfigureViews(this IServiceCollection services)
        {
            _ = services.AddControllersWithViews()
                   .AddViewLocalization()
                   .AddDataAnnotationsLocalization(options =>
                   {
                       options.DataAnnotationLocalizerProvider = (type, factory) =>
                       {
                           AssemblyName assemblyName = new(typeof(ResourcesFile).GetTypeInfo().Assembly.FullName);
                           return factory.Create(nameof(ResourcesFile), assemblyName.Name);
                       };
                   })
                   .AddSessionStateTempDataProvider()
                   .AddRazorRuntimeCompilation()
                   .AddNewtonsoftJson(options =>
                   {
                       options.SerializerSettings.Converters.Add(new StringEnumConverter());
                   });
        }

        public static void ConfigureSessionAndCookie(this IServiceCollection services)
        {
            _ = services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(1);
                options.IOTimeout = TimeSpan.FromMinutes(5);

                options.Cookie.Name = ".dashboard.cookie";
                options.Cookie.MaxAge = TimeSpan.FromDays(20);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            });

            _ = services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
            });

            _ = services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
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
    }
}
