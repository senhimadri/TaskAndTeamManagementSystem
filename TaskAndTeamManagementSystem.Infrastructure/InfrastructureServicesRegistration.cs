using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.Cachings;
using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.Notifications;
using TaskAndTeamManagementSystem.Infrastructure.Cachings;
using TaskAndTeamManagementSystem.Infrastructure.Logging;
using TaskAndTeamManagementSystem.Infrastructure.PushNotifications;

namespace TaskAndTeamManagementSystem.Infrastructure;

public static class InfrastructureServicesRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        SerilogConfiguration.ConfigureSerilog(configuration);

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
            options.InstanceName = "TaskTeamApp_";
        });

        services.AddSingleton<IRealTimeNotificationService, SignalRNotifier>();
        services.AddScoped<ICacheService, CacheService>();
        services.AddSignalR();
        return services;
    }
}
