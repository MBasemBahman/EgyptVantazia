using System.Net;

namespace API.Extensions
{
    public static class ExceptionMiddleExtensions
    {
        public static void ConfigureExceptionHandler(
            this IApplicationBuilder app,
            ILoggerManager logger)
        {
            _ = app.UseExceptionHandler(appError =>
            {
                appError.Run(context =>
                {
                    Response details = new();

                    IExceptionHandlerFeature contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        details = logger.LogError(context.Request, contextFeature.Error);
                    }

                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.Headers.Add(HeadersConstants.Status, details.ToString());
                    return Task.CompletedTask;
                });
            });
        }
    }
}
