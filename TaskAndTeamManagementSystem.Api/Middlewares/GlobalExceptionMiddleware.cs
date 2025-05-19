using Serilog;
using System.Text.Json;

namespace TaskAndTeamManagementSystem.Api.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _env;

    public GlobalExceptionMiddleware(RequestDelegate next, IWebHostEnvironment env)
    {
        _next = next;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "❌ Unhandled exception at {Path}", context.Request.Path);

            if (context.Response.HasStarted)
            {
                Log.Warning("❗ Response has already started, skipping error handling.");
                return;
            }

            context.Response.Clear();
            context.Response.ContentType = "application/json";

            await HandleExceptionAsync(context, ex);
        }
    }


    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, message) = exception switch
        {
            ArgumentNullException => (StatusCodes.Status400BadRequest, "A required parameter was null."),
            ArgumentException => (StatusCodes.Status400BadRequest, exception.Message),
            KeyNotFoundException => (StatusCodes.Status404NotFound, "The requested resource was not found."),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized access."),
            NotImplementedException => (StatusCodes.Status501NotImplemented, "Not implemented."),
            OperationCanceledException => (StatusCodes.Status408RequestTimeout, "Request timed out."),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
        };

        context.Response.StatusCode = statusCode;

        var response = new
        {
            StatusCode = statusCode,
            Error = message
        };

        var json = JsonSerializer.Serialize(response);

        try
        {
            return context.Response.WriteAsync(json);
        }
        catch (Exception ex)
        {
            throw;
        }

    }
}
