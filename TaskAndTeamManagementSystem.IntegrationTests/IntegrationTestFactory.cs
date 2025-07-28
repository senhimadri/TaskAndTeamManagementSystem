using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Moq;
using System.Net.Http.Headers;
using TaskAndTeamManagementSystem.Application.Contracts.Identities;
using TaskAndTeamManagementSystem.Application.Contracts.Identities.IRepositories;
using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.Cachings;
using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.MessageBrokers;
using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.Notifications;
using TaskAndTeamManagementSystem.Persistence;

namespace TaskAndTeamManagementSystem.IntegrationTests;

internal class IntegrationTestFactory : WebApplicationFactory<Program>, IAsyncLifetime 
{

    private readonly IConfiguration _configuration;
    private readonly string _environment;

    private readonly string _connectionString;
    public IntegrationTestFactory(string environment = "Test")
    {

        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        _configuration = builder;
        _environment = environment;

        _connectionString = _configuration.GetConnectionString("AppDbConnection")
            ?? throw new InvalidOperationException("AppDbConnection string not configured.");
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment(_environment)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                         .AddJsonFile($"appsettings.{_environment}.json", optional: true, reloadOnChange: true)
                         .AddEnvironmentVariables();
            })
            .ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<AppDbContext>));

                services.AddDbContext<AppDbContext>(options =>
                       options.UseSqlServer(_connectionString));


                services.AddSingleton<IRealTimeNotificationService>(Mock.Of<IRealTimeNotificationService>(m =>
                       m.SendNotificationAsync(It.IsAny<Guid>(), It.IsAny<string>()) == Task.CompletedTask));

                services.AddSingleton<IEventPublisher>(Mock.Of<IEventPublisher>(m =>
                    m.PublishAsync(It.IsAny<object>()) == Task.CompletedTask));

                services.AddSingleton<ICacheService>(Mock.Of<ICacheService>(m =>
                    m.GetAsync<object>(It.IsAny<string>()) == Task.FromResult<object>(null) &&
                    m.SetAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<TimeSpan?>()) == Task.CompletedTask));


                var serviceProvider = services.BuildServiceProvider();
                using var scope = serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.EnsureCreated();

            });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        return host;

    }

    public HttpClient CreateAuthenticatedClient(string role)
    {
        var client = CreateClient();    

        var email = role switch
        {
            "Admin" => "admin@demo.com",
            "Manager" => "manager@demo.com",
            "Employee" => "employee@demo.com",
            _ => throw new ArgumentException("Invalid role", nameof(role))
        };

        using var scope = Services.CreateScope();
        var identityService = scope.ServiceProvider.GetRequiredService<IIdentityUnitOfWork>();
        var tokenUtils = scope.ServiceProvider.GetRequiredService<ITokenUtils>();

        var user = identityService.UserManager.FindByEmailAsync(email).GetAwaiter().GetResult();
        if (user is null)
        {
            throw new InvalidOperationException($"User with email {email} not found.");
        }
        var roles = identityService.UserManager.GetRolesAsync(user).GetAwaiter().GetResult();

        var token = tokenUtils.GenerateAccessToken(user, roles);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return client;
    }

    public async Task InitializeAsync()
    {
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (!await dbContext.Database.CanConnectAsync())
        {
            throw new InvalidOperationException("Cannot connect to the test database.");
        }
    }
    public AppDbContext CreateDbContext()
    {
        var scope = Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<AppDbContext>();
    }

    public Task DisposeAsync()
    {
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.EnsureDeleted();
        return Task.CompletedTask;
    }
}

