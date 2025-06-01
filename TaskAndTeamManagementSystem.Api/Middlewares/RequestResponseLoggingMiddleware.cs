using Serilog;
using System.Diagnostics;
using System.Text;

namespace TaskAndTeamManagementSystem.Api.Middlewares;

public class RequestResponseLoggingMiddleware(RequestDelegate _next)
{
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
        var requestLog = await CaptureRequestDetails(context.Request);

        try
        {

            await _next(context);
            stopwatch.Stop();

            if (!context.Response.HasStarted)
            {
                var responseLog = await CaptureResponseDetails(context.Response,stopwatch.ElapsedMilliseconds);

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
        catch (Exception ex)
        {
            var combinedLog = new
            {
                Request = requestLog,
                Response = ex.Message
            };
            Log.Error(ex, "🔄 Request-Response: {CombinedLog}", combinedLog);

            throw;
        }
        finally
        {
            if (responseBody.Length > 0)
            {
                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
            }
            context.Response.Body = originalBodyStream;
        }
    }

    private async Task<object?> CaptureRequestDetails(HttpRequest request)
    {
        try
        {
            var body = await ReadRequestBody(request);
            return new
            {
                Method = request.Method,
                Url = request.Path.ToString(),
                Body = body
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error in RequestResponseLoggingMiddleware");
            return null;
        }

    }
    private async Task<object> CaptureResponseDetails(HttpResponse response, long elapsedTimeMs)
    {
        if (response.Body.CanSeek)
        {
            response.Body.Seek(0, SeekOrigin.Begin);

            using var reader = new StreamReader(stream: response.Body, encoding: Encoding.UTF8,
                                                detectEncodingFromByteOrderMarks: false,
                                                bufferSize: 1024, leaveOpen: true);

            var body = await reader.ReadToEndAsync();

            response.Body.Seek(0, SeekOrigin.Begin);

            return new
            {
                StatusCode = response.StatusCode,
                ElapsedTimeMs = elapsedTimeMs,
                Body = body
            };
        }
        Log.Warning("Response body stream is not seekable, skipping response body logging.");
        return new
        {
            StatusCode = response.StatusCode,
            ElapsedTimeMs = elapsedTimeMs,
            Body = string.Empty
        };
    }

    private static async Task<string> ReadRequestBody(HttpRequest request)
    {
        request.EnableBuffering();
        try
        {
            using var reader = new StreamReader(stream: request.Body, encoding: Encoding.UTF8,
                                    detectEncodingFromByteOrderMarks: false, bufferSize: 1024, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            return string.IsNullOrEmpty(body) ? string.Empty : body;
        }
        finally
        {
            request.Body.Position = 0;
        }
    }
}

