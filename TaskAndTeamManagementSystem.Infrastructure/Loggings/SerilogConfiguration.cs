using Microsoft.Extensions.Configuration;
using Serilog;

namespace TaskAndTeamManagementSystem.Infrastructure.Logging;

public static class SerilogConfiguration
{
    public static void ConfigureSerilog(IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.WithThreadId()
            .Enrich.WithMachineName()
            .Enrich.WithProcessId()
            .CreateLogger();
    }
}
