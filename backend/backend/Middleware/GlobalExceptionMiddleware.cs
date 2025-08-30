using System.Net;
using System.Text.Json;
using Serilog;

namespace backend.Middleware
{
    /// <summary>
    /// Middleware global para manejo centralizado de excepciones
    /// Captura todas las excepciones no manejadas y devuelve las respuestas
    /// </summary>
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
            var requestId = context.TraceIdentifier;
            var method = context.Request.Method;
            var path = context.Request.Path;
            var queryString = context.Request.QueryString.ToString();

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error no manejado en la aplicación para {Method} {Path}{QueryString} con RequestId {RequestId}", 
                    method, path, queryString, requestId);
                
                await HandleExceptionAsync(context, ex, requestId);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception, string requestId)
        {
            context.Response.ContentType = "application/json";
            
            var response = new
            {
                message = "Error interno del servidor",
                error = exception.Message,
                requestId = requestId,
                timestamp = DateTime.UtcNow
            };

            // Determinar el código de estado HTTP apropiado
            context.Response.StatusCode = exception switch
            {
                ArgumentException => (int)HttpStatusCode.BadRequest,
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }
    }

    /// <summary>
    /// Extensión para registrar el middleware de excepciones globales
    /// </summary>
    public static class GlobalExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}
