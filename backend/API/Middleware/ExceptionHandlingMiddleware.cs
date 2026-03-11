using System.Net;
using System.Text.Json;
using FluentValidation;

namespace API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
                await HandleExceptionAsync(context, ex, _logger);
            }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception,
            ILogger logger)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            var errorCode = "UNEXPECTED_ERROR";
            string message = "Ocorreu um erro inesperado. Tente novamente mais tarde.";

            switch (exception)
            {
                case ValidationException validationException:
                    statusCode = HttpStatusCode.BadRequest;
                    errorCode = "VALIDATION_ERROR";
                    message = validationException.Message;
                    break;
                case KeyNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    errorCode = "NOT_FOUND";
                    message = "Recurso não encontrado.";
                    break;
            }

            logger.LogError(exception, "Unhandled exception: {ErrorCode}", errorCode);

            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var responseBody = new
            {
                message,
                errorCode,
                traceId = context.TraceIdentifier
            };

            var json = JsonSerializer.Serialize(responseBody);
            await context.Response.WriteAsync(json);
        }
    }
}

