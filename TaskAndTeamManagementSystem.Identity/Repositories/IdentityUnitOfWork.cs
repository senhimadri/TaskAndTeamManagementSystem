using Microsoft.AspNetCore.Identity;
using TaskAndTeamManagementSystem.Application.Contracts.Identities;
using TaskAndTeamManagementSystem.Domain;
using TaskAndTeamManagementSystem.Persistence;

namespace TaskAndTeamManagementSystem.Identity.Repositories;


public class IdentityUnitOfWork(AppDbContext context,
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    RoleManager<ApplicationRole> roleManager) : IIdentityUnitOfWork
{
    private bool _disposed = false;

    private readonly AppDbContext _context = context;
    public UserManager<ApplicationUser> UserManager { get; } = userManager;
    public SignInManager<ApplicationUser> SignInManager { get; } = signInManager;
    public RoleManager<ApplicationRole> RoleManager { get; } = roleManager;

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }
    }
}

