namespace Dashboard.Middlewares
{
    public class HeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public HeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            IServiceProvider services = context.RequestServices;
            AppSettings _appSettings = services.GetService<IOptions<AppSettings>>().Value;

            context.Request.Headers.Add(HeadersConstants.AppKey, _appSettings.AppKey);

            await _next.Invoke(context);
        }
    }
}
