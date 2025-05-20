using Serilog;
using System.Text;

namespace TaskAndTeamManagementSystem.Api.Helpers;

public static class HttpCaptureHelper
{
    public static async Task<object?> CaptureRequestDetails(this HttpRequest request)
    {
        try
        {
            var body = await ReadRequestBody(request);

            return new
            {
                Method = request.Method,
                Url = request.Path.ToString(),
                Headers = request.Headers.ToDictionary(
                            h => h.Key,
                            h => h.Value.ToString(),
                            StringComparer.OrdinalIgnoreCase
                        ),
                Body = body
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error in RequestResponseLoggingMiddleware");
            return null;
        }

    }
    public static async Task<object> CaptureResponseDetails(this HttpResponse response, long elapsedTimeMs)
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
