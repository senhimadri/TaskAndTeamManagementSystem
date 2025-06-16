using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TaskAndTeamManagementSystem.Domain;
using TaskAndTeamManagementSystem.Persistence;

namespace TaskAndTeamManagementSystem.Identity;

public static class IdentitySeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Ensure database is created
        await context.Database.EnsureCreatedAsync();


        string[] roleNames = { "Admin", "Manager", "Employee", "Guest" };

        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var role = new ApplicationRole
                {
                    Id = Guid.NewGuid(),
                    Name = roleName,
                    NormalizedName = roleName.ToUpperInvariant()
                };

                var roleResult = await roleManager.CreateAsync(role);
                if (!roleResult.Succeeded)
                {
                    throw new Exception($"Failed to create role");
                }
            }
        }

        var users = new[]
        {
                new { Email = "admin@demo.com", Password = "Admin123!", FullName = "Admin User", Role = "Admin" },
                new { Email = "manager@demo.com", Password = "Manager123!", FullName = "Manager User", Role = "Manager" },
                new { Email = "employee@demo.com", Password = "Employee123!", FullName = "Employee User", Role = "Employee" },
                new { Email = "employee-01@demo.com", Password = "Employee123!", FullName = "Employee 01 User", Role = "Employee" },
                new { Email = "employee-02@demo.com", Password = "Employee123!", FullName = "Employee 02 User", Role = "Employee" },
                new { Email = "employee-03@demo.com", Password = "Employee123!", FullName = "Employee 03 User", Role = "Employee" },
        };

        // Seed users
        foreach (var userData in users)
        {
            var user = await userManager.FindByEmailAsync(userData.Email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = userData.Email,
                    Email = userData.Email,
                    Name = userData.FullName,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, userData.Password);

                if (result.Succeeded)
                {
                    var roleResult = await userManager.AddToRoleAsync(user, userData.Role);
                    if (!roleResult.Succeeded)
                    {
                        throw new Exception($"Failed to assign role {userData.Role} to user {userData.Email}");
                    }
                }
                else
                {
                    throw new Exception($"Failed to create user {userData.Email}");
                }
            }
        }

        await context.SaveChangesAsync();
    }

    public static async Task AddIdentitySeedData(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            await InitializeAsync(services);
        }
        catch (Exception)
        {
            throw;
        }
    }
}

