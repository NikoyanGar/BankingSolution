using System.Net;
using System.Text.Json;

namespace ScoringServiceAPI.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            HttpStatusCode statusCode;
            string message;

            switch (ex)
            {
                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized;
                    message = "Unauthorized";
                    break;
                case KeyNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    message = "Resource not found";
                    break;
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    message = "Internal Server Error";
                    break;
            }

            response.StatusCode = (int)statusCode;

            var result = JsonSerializer.Serialize(new
            {
                status = (int)statusCode,
                error = message,
                details = ex.Message
            });

            return response.WriteAsync(result);
        }

    }
}
