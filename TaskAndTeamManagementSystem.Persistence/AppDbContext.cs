using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskAndTeamManagementSystem.Domain;

namespace TaskAndTeamManagementSystem.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {

        var entityTypes = builder.Model.GetEntityTypes()
                            .Where(entityType => typeof(IBaseDomain).IsAssignableFrom(entityType.ClrType))
                            .Select(entityType => entityType.ClrType);

        foreach (var clrType in entityTypes)
        {
            var parameter = Expression.Parameter(clrType, "e");
            var property = Expression.Property(parameter, nameof(IBaseDomain.IsDelete));
            var notDeleted = Expression.Equal(property, Expression.Constant(false));

            var lambda = Expression.Lambda(notDeleted, parameter);

            builder.Entity(clrType).HasQueryFilter(lambda);
        }

        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
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
    public DbSet<TaskItem> TaskItems { get; set; } = default!;
    public DbSet<Team> Teams { get; set; } = default!;
}
