namespace Dashboard.Middlewares
{
    public sealed class BodyBufferingMiddleware
    {
        private readonly RequestDelegate _next;

        public BodyBufferingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try { context.Request.EnableBuffering(); } catch { }
            await _next(context);
            // context.Request.Body.Dipose() might be added to release memory, not tested
        }
    }
}
