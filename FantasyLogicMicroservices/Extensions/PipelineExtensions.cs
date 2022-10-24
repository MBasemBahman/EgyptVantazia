namespace FantasyLogicMicroservices.Extensions
{
    public static class PipelineExtensions
    {
        public static void ConfigureStaticFiles(this WebApplication app)
        {
            _ = app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context
                       .Response
                       .Headers
                       .Append("Access-Control-Allow-Origin", "*");

                    ctx.Context
                       .Response
                       .Headers
                       .Append("Access-Control-Allow-Headers", "Origin, x-Requested-With, Content-Type, Accept");
                }
            });
        }

        public static void ConfigureSwagger(this WebApplication app, TenantConfig config)
        {
            _ = app.UseSwagger();
            _ = app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/Season/swagger.json", "Season");
                c.SwaggerEndpoint($"/swagger/Standings/swagger.json", "Standings");
                c.SwaggerEndpoint($"/swagger/Team/swagger.json", "Team");
                c.SwaggerEndpoint($"/swagger/Games/swagger.json", "Games");

                c.RoutePrefix = "docs";
            });
        }
    }
}
