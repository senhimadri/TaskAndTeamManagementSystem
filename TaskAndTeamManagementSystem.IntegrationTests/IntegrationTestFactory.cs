using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskAndTeamManagementSystem.Application.Contracts.Identities.IRepositories;
using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.Cachings;
using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.MessageBrokers;
using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.Notifications;
using TaskAndTeamManagementSystem.Identity;
using TaskAndTeamManagementSystem.Persistence;

namespace TaskAndTeamManagementSystem.IntegrationTests;

internal class IntegrationTestFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly string _connectionString = "Server=Himaderi-560620;Database=TaskAndTeamManagementSystem-Test-Db;User Id=sa;Password=YourStrong@Passw0rd;MultipleActiveResultSets=true;TrustServerCertificate=True;";
    private readonly JwtSettings _jwtSettings;
    private Guid _seededUserId; 

    public IntegrationTestFactory()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["JwtSettings:Issuer"] = "https://yourdomain.com",
                ["JwtSettings:Audience"] = "youraudience",
                ["JwtSettings:Secret"] = "YourSuperSecretKeyAtLeast32CharactersLong",
                ["ConnectionStrings:AppDbConnection"] = _connectionString,
                ["ConnectionStrings:RedisConnection"] = "localhost:6379" // Mock Redis connection
            })
            .Build();
        _jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>()
            ?? throw new InvalidOperationException("JWT settings not configured for tests.");

    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["JwtSettings:Issuer"] = _jwtSettings.Issuer,
                ["JwtSettings:Audience"] = _jwtSettings.Audience,
                ["JwtSettings:Secret"] = _jwtSettings.Secret,
                ["ConnectionStrings:AppDbConnection"] = _connectionString,
                ["ConnectionStrings:RedisConnection"] = "localhost:6379"
            });
        });

        builder.ConfigureTestServices(services =>
        {
            // Configure real database
            services.RemoveAll(typeof(DbContextOptions<AppDbContext>));
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(_connectionString));

            // Register services
            services.AddSingleton<IRealTimeNotificationService>(Mock.Of<IRealTimeNotificationService>());
            services.AddSingleton<IEventPublisher>(Mock.Of<IEventPublisher>());
            services.AddSingleton<ICacheService>(Mock.Of<ICacheService>());

            services.AddScoped<ICurrentUserService>(sp =>
            {
                using var scope = sp.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var user = dbContext.Users.First(u => u.Email == "admin@demo.com"); // Match seeded user
                _seededUserId = user.Id; // Store for token generation

                return new MockCurrentUserService(
                    user.Id,
                    user.UserName,
                    user.Email,
                    new [] { "Admin" }
                );
            });

            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.EnsureCreated(); // Creates the database if it doesn't exist
            //dbContext.Database.Migrate(); // Applies migrations if needed
        });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);
        return host;
    }

    public string GenerateJwtToken(string email, string role)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, _seededUserId.ToString()),
            new Claim(ClaimTypes.Name, email),
            new Claim(ClaimTypes.Role, role),
            new Claim("sub", _seededUserId.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateGoogleOAuth2Token(string email, string role)
    {
        var claims = new[]
        {
            new Claim("sub", email),
            new Claim(ClaimTypes.Name, email),
            new Claim(ClaimTypes.Role, role),
            new Claim("iss", "accounts.google.com"),
            new Claim("aud", _jwtSettings.Audience),
            new Claim("email", email),
            new Claim("email_verified", "true")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "accounts.google.com",
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task InitializeAsync()
    {
        // Ensure the database is ready and migrations are applied
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (!await dbContext.Database.CanConnectAsync())
        {
            throw new InvalidOperationException("Cannot connect to the test database.");
        }
    }

    public Task DisposeAsync()
    {
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.EnsureDeleted(); // Clean up the test database
        return Task.CompletedTask;
    }


    //public override async ValueTask DisposeAsync()
    //{
    //    using var scope = Services.CreateScope();
    //    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    //    await dbContext.Database.EnsureDeletedAsync(); // Clean up the test database asynchronously
    //    GC.SuppressFinalize(this); // Prevent finalizer from being called
    //}

    public AppDbContext CreateDbContext()
    {
        var scope = Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<AppDbContext>();
    }
}

