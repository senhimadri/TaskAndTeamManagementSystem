using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace TaskAndTeamManagementSystem.Application;

public static class ApplicationServicesRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddHttpClient();

        return services;
    }
}
