namespace FantasyLogicMicroservices.Middlewares
{
    public class ThemeMiddleware
    {
        private readonly RequestDelegate _next;

        public ThemeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string theme = context.Request.Headers[HeadersConstants.Theme].FirstOrDefault()?.Split(" ").Last();

            context.Items[ApiConstants.Theme] = theme.SafeLower() == "dark";

            await _next(context);
        }
    }
}
