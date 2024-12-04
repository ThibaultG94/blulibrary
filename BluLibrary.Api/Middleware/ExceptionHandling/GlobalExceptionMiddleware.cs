using System.Net;
using System.Text.Json;
using BluLibrary.Core.Exceptions;

namespace BluLibrary.Api.Middleware.ExceptionHandling;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // On configure la réponse HTTP de base
        context.Response.ContentType = "application/json";
        var response = new ApiErrorResponse();

        // On adapte la réponse selon le type d'exception
        switch (exception)
        {
            case DomainValidationException validationEx:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Message = validationEx.Message;
                _logger.LogWarning(validationEx, "Validation error occurred");
                break;

            case KeyNotFoundException notFoundEx:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                response.Message = notFoundEx.Message;
                _logger.LogWarning(notFoundEx, "Resource not found");
                break;

            default:
                // Pour toute autre erreur, on retourne une 500
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Message = _environment.IsDevelopment() 
                    ? exception.ToString() 
                    : "Une erreur interne est survenue.";
                _logger.LogError(exception, "Une erreur non gérée est survenue");
                break;
        }

        // On ajoute des détails supplémentaires en développement
        if (_environment.IsDevelopment())
        {
            response.DeveloperMessage = new
            {
                exception.StackTrace,
                ExceptionType = exception.GetType().Name
            };
        }

        // On sérialise et on envoie la réponse
        var result = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        });

        await context.Response.WriteAsync(result);
    }
}

// Classe qui définit la structure de notre réponse d'erreur
public class ApiErrorResponse
{
    public string Message { get; set; } = string.Empty;
    public object? DeveloperMessage { get; set; }
    public string RequestId { get; set; } = Guid.NewGuid().ToString();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

// Extension pour faciliter l'ajout du middleware
public static class GlobalExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(
        this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalExceptionMiddleware>();
    }
}