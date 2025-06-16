using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.Cachings;
using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.MessageBrokers;
using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.Notifications;
using TaskAndTeamManagementSystem.Infrastructure.Cachings;
using TaskAndTeamManagementSystem.Infrastructure.MessageBrokers.Configurations;
using TaskAndTeamManagementSystem.Infrastructure.MessageBrokers.Publishers;
using TaskAndTeamManagementSystem.Infrastructure.PushNotifications;
using TaskAndTeamManagementSystem.Persistence;

namespace TaskAndTeamManagementSystem.Infrastructure;

public static class InfrastructureServicesRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("RedisConnection");
            options.InstanceName = "TaskTeamApp_";
        });

        services.AddMassTransitWithRabbitMQConfiguration<AppDbContext>(configuration);
        services.AddScoped<IEventPublisher, MassTransitEventPublisher>();

        services.AddSignalR();
        services.AddSingleton<IRealTimeNotificationService, SignalRNotificationService>();

        services.AddScoped<ICacheService, CacheService>();

        return services;
    }
}
