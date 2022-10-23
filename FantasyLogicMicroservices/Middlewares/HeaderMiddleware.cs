namespace FantasyLogicMicroservices.Middlewares
{
    public class HeaderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IJwtUtils _jwtUtils;

        private readonly int _expires;
        public HeaderMiddleware(RequestDelegate next, IJwtUtils jwtUtils)
        {
            _next = next;
            _jwtUtils = jwtUtils;
            _expires = 60;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.Headers.Add(HeadersConstants.Validator, _jwtUtils.GenerateJwtToken(_expires, _expires).RefreshToken);

            context.Response.Headers.Add(HeadersConstants.Status, new Response(true).ToString());
            await _next.Invoke(context);
        }
    }
}
