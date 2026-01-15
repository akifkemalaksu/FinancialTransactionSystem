using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ServiceDefaults.Dtos.Responses;
using System.Text.Json;

namespace ServiceDefaults.Middlewares
{
    public class ExceptionHandlingMiddleware(
        RequestDelegate _next,
        ILogger<ExceptionHandlingMiddleware> _logger
    )
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unhandled exception occurred while processing request {Method} {Path}",
                    context.Request.Method,
                    context.Request.Path
                );

                await HandleExceptionAsync(context);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var response = ApiResponse<NoContent>.Failure(
                StatusCodes.Status500InternalServerError,
                "An unexpected error occurred."
            );

            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}

