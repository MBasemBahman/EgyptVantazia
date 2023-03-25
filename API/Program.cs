using API.MappingProfileCls;
using Entities.ServicesModels;

TenantConfig config = new(TenantEnvironments.Development);

LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), config.NlogConfig));

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile(config.AppSettings);

// Add services to the container
builder.Services.ConfigureRequestsLimit();
builder.Services.ConfigureCors();
builder.Services.AddHttpClient();
builder.Services.ConfigureLoggerService();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<PaymobConfiguration>(builder.Configuration.GetSection("PaymobConfiguration"));
builder.Services.AddSingleton(provider => new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new MappingProfile(provider.GetService<IOptions<AppSettings>>()));
}).CreateMapper());
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureLocalization();
builder.Services.ConfigureVersioning();
builder.Services.ConfigureResponseCaching();
builder.Services.ConfigureSingletonService();
builder.Services.ConfigureScopedService();

builder.Services.ConfigureSqlContext(builder.Configuration, config);
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.ConfigureSwagger(config);

builder.Services.AddControllers(opt =>
{
    _ = opt.Filters.Add(typeof(GlobalModelStateValidatorAttribute));
});
builder.Services.ConfigureFirebase(config.AppSettings);
builder.Services.ConfigureEmailSender(builder.Configuration);
builder.Services.ConfigurePaymob(builder.Configuration);
//builder.Services.ConfigureHangfire(builder.Configuration);

WebApplication app = builder.Build();

app.UseIpRateLimiting();

ILoggerManager logger = app.Services.GetRequiredService<ILoggerManager>();
ILocalizationManager localizer = app.Services.GetRequiredService<ILocalizationManager>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();
app.ConfigureStaticFiles();
app.UseFileServer();
app.UseCors();
app.ConfigureSwagger(config);
app.UseResponseCaching();
app.UseRequestLocalization(app.Services.GetService<IOptions<RequestLocalizationOptions>>().Value);

app.UseMiddleware<BodyBufferingMiddleware>();
app.UseMiddleware<JwtMiddleware>();
app.UseMiddleware<CultureMiddleware>();
app.UseMiddleware<ThemeMiddleware>();
app.UseMiddleware<HeaderMiddleware>();
app.ConfigureExceptionHandler(logger);

app.UseRouting();

//app.UseHangfireDashboard("/schedulejobs", new DashboardOptions
//{
//    AppPath = "",
//    DashboardTitle = "schedule jobs",
//});

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();
});

app.Run();
