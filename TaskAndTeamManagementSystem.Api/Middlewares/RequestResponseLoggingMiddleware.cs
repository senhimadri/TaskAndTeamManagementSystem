using Serilog;
using System.Diagnostics;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace TaskAndTeamManagementSystem.Api.Middlewares;

public class RequestResponseLoggingMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;
    public async Task Invoke(HttpContext context)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();

            var requestBody = await ReadRequestBody(context.Request);
            var requestLog = new
            {
                Method = context.Request.Method,
                Url = context.Request.Path,
                Headers = context.Request.Headers,
                Body = requestBody
            };

            Log.Information("➡️ Request: {Request}", requestLog);

            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            stopwatch.Stop();

            var responseBodyContent = await ReadResponseBody(context.Response);
            var responseLog = new
            {
                StatusCode = context.Response.StatusCode,
                ElapsedTimeMs = stopwatch.ElapsedMilliseconds,
                Body = responseBodyContent ?? string.Empty
            };

            Log.Information("⬅️ Response: {Response}", responseLog);

            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
        }
        catch (Exception ex)
        {
            throw;
        }
        


    }

    private async Task<string> ReadRequestBody(HttpRequest request)
    {
        request.EnableBuffering();
        using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        request.Body.Position = 0;
        return body;
    }

    private async Task<string> ReadResponseBody(HttpResponse response)
    {
        try
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(stream: response.Body,
                                                encoding: Encoding.UTF8,
                                                detectEncodingFromByteOrderMarks: false,
                                                bufferSize: 1024,
                                                leaveOpen: true);

            var body = await reader.ReadToEndAsync();
            response.Body.Seek(offset: 0, origin: SeekOrigin.Begin);
            return body;
        }
        catch (Exception ex)
        {
            Log.Error(exception: ex, messageTemplate: "Failed to read response body.");
            return string.Empty;
        }
    }


    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        WriteIndented = true, // ✅ Makes JSON readable
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // ✅ Converts PascalCase to camelCase
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping // ✅ Prevents excessive escaping
    };
}

