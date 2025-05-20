using Serilog;
using System.Text.Json;
using TaskAndTeamManagementSystem.Api.Helpers;
using TaskAndTeamManagementSystem.Application.Helpers.Extensions;

namespace TaskAndTeamManagementSystem.Api.Middlewares;

public class GlobalExceptionMiddleware(RequestDelegate _next, IWebHostEnvironment _environment)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {

            var requestDetails =(await  context.Request.CaptureRequestDetails()).SerializeToJson();

            Log.Error(ex, "❌ Unhandled exception at {Path}. Request Details {RequestDetails}", context.Request.Path, requestDetails);

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

        return context.Response.WriteAsync(json);
    }
}
