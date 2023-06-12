TenantConfig config = new(TenantEnvironments.Development);

LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), config.NlogConfig));

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile(config.AppSettings);

// Add services to the container.

builder.Services.ConfigureCors();
builder.Services.AddHttpClient();
builder.Services.ConfigureLoggerService();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureLocalization();
builder.Services.ConfigureResponseCaching();
builder.Services.ConfigureScopedService();
builder.Services.ConfigureSqlContext(builder.Configuration, config);
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.ConfigureViews();
builder.Services.ConfigureSessionAndCookie();
builder.Services.ConfigureFirebase(config.AppSettings);

WebApplication app = builder.Build();

_ = app.UseDeveloperExceptionPage();

////Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    _ = app.UseDeveloperExceptionPage();
//}
//else
//{
//    _ = app.UseExceptionHandler("/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    _ = app.UseHsts();
//}

app.UseHttpsRedirection();
app.ConfigureStaticFiles();
app.UseFileServer();
app.UseRouting();
app.UseCors();
app.UseSession();
app.UseCookiePolicy();
app.UseResponseCaching();
app.UseRequestLocalization(app.Services.GetService<IOptions<RequestLocalizationOptions>>().Value);

app.UseMiddleware<BodyBufferingMiddleware>();
app.UseMiddleware<JwtMiddleware>();
app.UseMiddleware<HeaderMiddleware>();
app.UseMiddleware<CultureMiddleware>();

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllerRoute(
        name: "areaRoute",
        pattern: "{area=Dashboard}/{controller=Authentication}/{action=Login}/{id?}"
    );
});

app.Run();
