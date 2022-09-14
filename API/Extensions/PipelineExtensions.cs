using Entities.TenantModels;

namespace API.Extensions
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
                foreach (SwaggerModel page in config.SwaggerPages)
                {
                    c.SwaggerEndpoint($"/swagger/{page.Name}/swagger.json", page.Name);
                }

                c.RoutePrefix = "docs";
            });
        }
    }
}
