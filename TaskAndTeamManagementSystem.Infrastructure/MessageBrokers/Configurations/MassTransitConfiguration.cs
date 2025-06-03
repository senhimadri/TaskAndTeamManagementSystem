using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace TaskAndTeamManagementSystem.Infrastructure.MessageBrokers.Configurations;

internal static class MassTransitConfiguration
{
    public static IServiceCollection AddMassTransitWithRabbitMQConfiguration<TDbContext>(this IServiceCollection services , IConfiguration configuration)
                                                                                            where TDbContext : DbContext
    {
        services.AddMassTransit(config =>
        {
            config.AddConsumers(Assembly.GetEntryAssembly());

            config.AddEntityFrameworkOutbox<TDbContext>(options =>
            {
                options.QueryDelay = TimeSpan.FromSeconds(10);
                options.DisableInboxCleanupService();
                options.UseSqlServer();
                options.UseBusOutbox();
            });

            config.UsingRabbitMq((context, configurator) =>
            {
                var rabbitMqSettings = configuration!.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();

                configurator.Host(rabbitMqSettings!.Host, h =>
                {
                    h.Username(rabbitMqSettings.UserName);
                    h.Password(rabbitMqSettings.Password);
                });

                configurator.ConfigureEndpoints(registration: context,
                            endpointNameFormatter: new KebabCaseEndpointNameFormatter(prefix: "TaskAndTeamManagementSystem", includeNamespace: false),
                            configureFilter: endpointConfigurator =>
                            {
                                if (endpointConfigurator is IReceiveEndpointConfigurator receiveEndpointConfigurator)
                                {
                                    receiveEndpointConfigurator.UseEntityFrameworkOutbox<TDbContext>(context);
                                }
                            }
                            );

                configurator.UseMessageRetry(retryConfig =>
                {
                    retryConfig.Interval(3, TimeSpan.FromSeconds(5));
                });
            });
        });

        return services;
    }
}
