using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net.Http.Headers;
using TaskAndTeamManagementSystem.Application;
using TaskAndTeamManagementSystem.Application.Contracts.Identities;
using TaskAndTeamManagementSystem.Application.Contracts.Identities.IRepositories;
using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.Cachings;
using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.MessageBrokers;
using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.Notifications;
using TaskAndTeamManagementSystem.Domain;
using TaskAndTeamManagementSystem.Identity;
using TaskAndTeamManagementSystem.Infrastructure;
using TaskAndTeamManagementSystem.Persistence;

namespace TaskAndTeamManagementSystem.IntegrationTests;

internal class IntegrationTestFactory : WebApplicationFactory<Program>, IAsyncLifetime 
{

    private readonly IConfiguration _configuration;
    private readonly string _environment;
    private readonly Dictionary<string, Guid> _seededUserIds = new Dictionary<string, Guid>();
    private readonly Lazy<HttpClient> _adminClient;
    private readonly Lazy<HttpClient> _managerClient;
    private readonly Lazy<HttpClient> _normalUserClient;

    private readonly string _connectionString;
    public IntegrationTestFactory(string environment = "IntegrationTests")
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

        _adminClient = new Lazy<HttpClient>(() => CreateAuthenticatedClient("Admin"));
        _managerClient = new Lazy<HttpClient>(() => CreateAuthenticatedClient("Manager"));
        _normalUserClient = new Lazy<HttpClient>(() => CreateAuthenticatedClient("NormalUser"));

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

                services.AddPersistenceServices(_configuration);
                services.AddApplicationServices();
                services.AddInfrastructureServices(_configuration);
                services.AddIdentityServices(_configuration);


                services.AddSingleton<IRealTimeNotificationService>(Mock.Of<IRealTimeNotificationService>(m =>
                       m.SendNotificationAsync(It.IsAny<Guid>(), It.IsAny<string>()) == Task.CompletedTask));

                services.AddSingleton<IEventPublisher>(Mock.Of<IEventPublisher>(m =>
                    m.PublishAsync(It.IsAny<object>()) == Task.CompletedTask));

                services.AddSingleton<ICacheService>(Mock.Of<ICacheService>(m =>
                    m.GetAsync<object>(It.IsAny<string>()) == Task.FromResult<object>(null) &&
                    m.SetAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<TimeSpan?>()) == Task.CompletedTask));

                services.AddSingleton<ILogger<Program>>(Mock.Of<ILogger<Program>>());

                services.AddScoped<ICurrentUserService>(sp =>
                {
                    using var scope = sp.CreateScope();
                    var identityService = scope.ServiceProvider.GetRequiredService<IIdentityUnitOfWork>();


                    var user = identityService.UserManager.FindByEmailAsync("admin@demo.com")
                                                .GetAwaiter().GetResult();

                    if (user is null)
                    {
                        throw new InvalidOperationException("Seeded user 'admin@demo.com' not found.");
                    }
                    _seededUserIds["Admin"] = user.Id;

                    var roles = identityService.UserManager.GetRolesAsync(user)
                                    .GetAwaiter().GetResult()
                                    .ToList();

                    return new MockCurrentUserService(user.Id, user.UserName, user.Email, roles);
                });

                var serviceProvider = services.BuildServiceProvider();
                using var scope = serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.EnsureCreated();
                dbContext.Database.Migrate();

            });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        using var scope = host.Services.CreateScope();
        var identityService = scope.ServiceProvider.GetRequiredService<IIdentityUnitOfWork>();

        var roles = new[] { "Admin", "Manager", "NormalUser" };
        foreach (var role in roles)
        {
            if (! identityService.RoleManager.RoleExistsAsync(role).GetAwaiter().GetResult())
            {
                identityService.RoleManager.CreateAsync(new ApplicationRole { Name = role }).GetAwaiter().GetResult();
            }
        }

        var users = new[]
        {
            new { Email = "admin@demo.com", Password = "Admin123!", Role = "Admin" },
            new { Email = "manager@demo.com", Password = "Manager123!", Role = "Manager" },
            new { Email = "user@demo.com", Password = "User123!", Role = "NormalUser" }
        };

        foreach (var userData in users)
        {
            var user = identityService.UserManager.FindByEmailAsync(userData.Email).GetAwaiter().GetResult() ?? new ApplicationUser
            {
                UserName = userData.Email,
                Email = userData.Email,
                EmailConfirmed = true
            };
            if (identityService.UserManager
                .CreateAsync(user, userData.Password)
                .ConfigureAwait(false).GetAwaiter().GetResult()
                .Succeeded)
            {
                identityService.UserManager.AddToRoleAsync(user, userData.Role).GetAwaiter();
            }
            _seededUserIds[userData.Role] = user.Id;
        }

        return host;

    }


    private HttpClient CreateAuthenticatedClient(string role)
    {
        var client = CreateClient();
        if (_seededUserIds.TryGetValue(role, out var userId) && userId != Guid.Empty)
        {
            var email = role switch
            {
                "Admin" => "admin@demo.com",
                "Manager" => "manager@demo.com",
                "NormalUser" => "user@demo.com",
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
        }
        else
        {
            throw new InvalidOperationException($"User with role {role} not seeded or ID not set.");
        }
        return client;
    }


    public HttpClient AdminClient => _adminClient.Value;
    public HttpClient ManagerClient => _managerClient.Value;
    public HttpClient NormalUserClient => _normalUserClient.Value;

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

