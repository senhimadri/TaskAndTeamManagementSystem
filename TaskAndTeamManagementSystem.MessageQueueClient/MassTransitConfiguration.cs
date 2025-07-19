using MassTransit;
using System.Reflection;

namespace TaskAndTeamManagementSystem.MessageQueueClient;

internal static class MassTransitConfiguration
{
    public static IServiceCollection AddMassTransitWithRabbitMQConfiguration(this IServiceCollection services)
    {
        services.AddMassTransit(config =>
        {
            config.AddConsumers(Assembly.GetEntryAssembly());

            config.UsingRabbitMq((context, configurator) =>
            {
                var configuration = context.GetService<IConfiguration>();

                var rabbitMqSettings = configuration!.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();

                configurator.Host(rabbitMqSettings!.Host, h =>
                {
                    h.Username(rabbitMqSettings.UserName);
                    h.Password(rabbitMqSettings.Password);
                });

                configurator.ConfigureEndpoints(registration: context, endpointNameFormatter:
                                new KebabCaseEndpointNameFormatter(prefix: "TaskAndTeamManagementSystem.MessageQueueClient", includeNamespace: false));

                configurator.UseMessageRetry(retryConfig =>
                {
                    retryConfig.Interval(3, TimeSpan.FromSeconds(5));
                });
            });
        });

        return services;
    }
}
