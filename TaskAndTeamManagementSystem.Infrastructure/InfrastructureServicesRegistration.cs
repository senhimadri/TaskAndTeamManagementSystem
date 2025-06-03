using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.Cachings;
using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.MessageBrokers;
using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.Notifications;
using TaskAndTeamManagementSystem.Infrastructure.Cachings;
using TaskAndTeamManagementSystem.Infrastructure.Logging;
using TaskAndTeamManagementSystem.Infrastructure.MessageBrokers.Configurations;
using TaskAndTeamManagementSystem.Infrastructure.MessageBrokers.Publishers;
using TaskAndTeamManagementSystem.Infrastructure.PushNotifications;
using TaskAndTeamManagementSystem.Persistence;

namespace TaskAndTeamManagementSystem.Infrastructure;

public static class InfrastructureServicesRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        SerilogConfiguration.ConfigureSerilog(configuration);

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("RedisConnection");
            options.InstanceName = "TaskTeamApp_";
        });

        services.AddMassTransitWithRabbitMQConfiguration<AppDbContext>(configuration);

        services.AddSingleton<IRealTimeNotificationService, SignalRNotificationService>();
        services.AddScoped<IEventPublisher, MassTransitEventPublisher>();
        services.AddScoped<ICacheService, CacheService>();
        services.AddSignalR();
        return services;
    }
}
