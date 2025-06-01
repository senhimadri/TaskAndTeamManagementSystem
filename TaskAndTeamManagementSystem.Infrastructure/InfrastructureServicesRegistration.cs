using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskAndTeamManagementSystem.Infrastructure.Logging;

namespace TaskAndTeamManagementSystem.Infrastructure;

public static class InfrastructureServicesRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        SerilogConfiguration.ConfigureSerilog(configuration);
        services.AddSignalR();
        return services;
    }
}
