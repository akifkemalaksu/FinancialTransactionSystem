using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ServiceDefaults.Middlewares
{
    public class RequestResponseLoggingMiddleware(
        RequestDelegate _next,
        ILogger<RequestResponseLoggingMiddleware> _logger
    )
    {
        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();
            var requestBody = await ReadStreamAsync(context.Request.Body);
            context.Request.Body.Position = 0;

            _logger.LogInformation(
                "Handling HTTP request {Method} {Path} | Body: {Body}",
                context.Request.Method,
                context.Request.Path,
                requestBody
            );

            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            context.Response.Body.Position = 0;
            var responseBodyText = await ReadStreamAsync(context.Response.Body);
            context.Response.Body.Position = 0;

            _logger.LogInformation(
                "Handled HTTP request {Method} {Path} with status code {StatusCode} | Body: {Body}",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                responseBodyText
            );

            await responseBody.CopyToAsync(originalBodyStream);
        }

        private static async Task<string> ReadStreamAsync(Stream stream)
        {
            if (stream == null) return string.Empty;
            using var reader = new StreamReader(stream, leaveOpen: true);
            var text = await reader.ReadToEndAsync();
            
            return text;
        }
    }
}

