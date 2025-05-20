using Serilog;
using System.Diagnostics;
using TaskAndTeamManagementSystem.Api.Helpers;

namespace TaskAndTeamManagementSystem.Api.Middlewares;

public class RequestResponseLoggingMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        var stopwatch = Stopwatch.StartNew();

        var originalBodyStream = context.Response.Body;
        
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        try
        {
            await _next(context);
            stopwatch.Stop();

            var requestLog = await context.Request.CaptureRequestDetails();

            if (!context.Response.HasStarted)
            {
                var responseLog = await context.Response.CaptureResponseDetails(stopwatch.ElapsedMilliseconds);

                var combinedLog = new
                {
                    Request = requestLog,
                    Response = responseLog
                };
                Log.Information("🔄 Request-Response: {CombinedLog}", combinedLog);
            }
            else
            {
                Log.Warning("Response has already started, skipping response logging.");
                Log.Information("➡️ Request (response skipped): {Request}", requestLog);
            }
        }
        finally
        {
            stopwatch.Stop();

            if (responseBody.Length > 0)
            {
                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
            }
            context.Response.Body = originalBodyStream;
        }
    }
}

