namespace ClientLoanServiceAPI.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;

            _logger.LogInformation("Incoming Request: {method} {url} at {time}",
                request.Method,
                request.Path,
                DateTime.UtcNow);

            await _next(context);

            _logger.LogInformation("Outgoing Response: {statusCode} for {method} {url} at {time}",
                context.Response.StatusCode,
                request.Method,
                request.Path,
                DateTime.UtcNow);
        }
    }
}
