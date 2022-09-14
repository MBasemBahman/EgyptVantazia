namespace API.Middlewares
{
    public class CultureMiddleware
    {
        private readonly RequestDelegate _next;

        public CultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string culture = context.Request.Headers[HeadersConstants.Culture].FirstOrDefault()?.Split(" ").Last();

            context.Items[ApiConstants.Language] = culture.SafeLower() == "en";

            await _next(context);
        }
    }
}
