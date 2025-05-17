using TaskAndTeamManagementSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace TaskAndTeamManagementSystem.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public DbSet<User> Users { get; set; } = default!;
    public DbSet<TaskItem> TaskItems { get; set; } = default!;
    public DbSet<Team> Teams { get; set; } = default!;
}
