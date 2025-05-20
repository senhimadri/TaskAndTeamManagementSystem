using System.Text.Json;

namespace TaskAndTeamManagementSystem.Application.Helpers.Extensions;

public static class CommonExtensions
{
    public static string ToIsoDateString(this DateTimeOffset dateTimeOffset)
        => dateTimeOffset.ToString("yyyy-MM-dd");

    public static string ToIsoDateString(this DateTime dateTime)
        => dateTime.ToString("yyyy-MM-dd");

    public static string ToIsoDateString(this DateTime? dateTime)
        => dateTime?.ToString("yyyy-MM-dd") ?? string.Empty;

    public static List<int> ParseCommaSeparatedInts(this string? commaSeparatedValues)
    {
        if (string.IsNullOrEmpty(commaSeparatedValues))
            return new List<int>();
        return commaSeparatedValues.Split(',').Select(int.Parse).ToList();
    }

    public static string ToCommaSeparatedString(this List<int>? values)
    {
        if (values == null || values.Count == 0)
            return string.Empty;
        return string.Join(",", values);
    }

    public static string SerializeToJson<T>(this T? data)
    {
        if (data is not null)
        {
            return JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        return string.Empty;
    }
}
