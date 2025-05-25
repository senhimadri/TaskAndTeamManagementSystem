using TaskAndTeamManagementSystem.Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Linq.Expressions;

namespace TaskAndTeamManagementSystem.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        var entityTypes = modelBuilder.Model.GetEntityTypes()
            .Where(entityType => typeof(IBaseDomain).IsAssignableFrom(entityType.ClrType))
            .Select(entityType => entityType.ClrType);

        foreach (var clrType in entityTypes)
        {
            var parameter = Expression.Parameter(clrType, "e");
            var property = Expression.Property(parameter, nameof(IBaseDomain.IsDelete));
            var notDeleted = Expression.Equal(property, Expression.Constant(false));

            var lambda = Expression.Lambda(notDeleted, parameter);

            modelBuilder.Entity(clrType).HasQueryFilter(lambda);
        }

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
                        .Where(e => e.Entity is IBaseDomain &&
                        (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (IBaseDomain)entry.Entity;

            if (entry.State == EntityState.Added)
            {
                entity.CreateAt = DateTimeOffset.UtcNow;
                entity.IsDelete = false;
            }
            else if (entry.State == EntityState.Modified)
            {
                entity.UpdateAt = DateTimeOffset.UtcNow;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }

    public DbSet<User> Users { get; set; } = default!;
    public DbSet<TaskItem> TaskItems { get; set; } = default!;
    public DbSet<Team> Teams { get; set; } = default!;
}
