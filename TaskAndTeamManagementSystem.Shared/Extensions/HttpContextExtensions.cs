using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace TaskAndTeamManagementSystem.Shared.Extensions;

public static class HttpContextExtensions
{
    public static T? GetRouteValue<T>(this IHttpContextAccessor httpContextAccessor, string key)
    {
        var routeValues = httpContextAccessor.HttpContext?.GetRouteData().Values;
        if (routeValues == null || !routeValues.TryGetValue(key, out var value))
            return default;

        var valueStr = value?.ToString();
        if (string.IsNullOrEmpty(valueStr))
            return default;

        try
        {
            var targetType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

            if (targetType == typeof(Guid))
            {
                if (Guid.TryParse(valueStr, out var guidValue))
                    return (T)(object)guidValue;
                return default;
            }

            var convertedValue = Convert.ChangeType(valueStr, targetType);
            return (T)convertedValue;
        }
        catch
        {
            return default;
        }
    }
}
