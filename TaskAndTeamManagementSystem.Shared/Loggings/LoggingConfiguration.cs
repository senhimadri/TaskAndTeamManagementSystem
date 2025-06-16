using Microsoft.Extensions.Configuration;
using Serilog;

namespace TaskAndTeamManagementSystem.Shared.Logging;

public static class LoggingConfiguration
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
