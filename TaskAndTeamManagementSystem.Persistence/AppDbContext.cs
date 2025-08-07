using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Linq.Expressions;
using TaskAndTeamManagementSystem.Application.Contracts.Identities.IRepositories;
using TaskAndTeamManagementSystem.Domain;
using TaskAndTeamManagementSystem.Domain.Bases;
using TaskAndTeamManagementSystem.Domain.Identities;

namespace TaskAndTeamManagementSystem.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserService? currentUser = null)
                                                            : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, UserClaim, UserRole, UserLogin,RoleClaim, UserToken>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {

        var entityTypes = builder.Model.GetEntityTypes()
                            .Where(entityType => typeof(IBaseDomain).IsAssignableFrom(entityType.ClrType))
                            .Select(entityType => entityType.ClrType);

        foreach (var clrType in entityTypes)
        {
            var lambda = GetIsDeletedRestriction(clrType);
            builder.Entity(clrType).HasQueryFilter(lambda);
        }

        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries().Where(e => e.Entity is IBaseDomain);

        foreach (var entry in entries)
        {
            var entity = (IBaseDomain)entry.Entity;

            switch (entry.State)
            {
                case EntityState.Added:
                        entity.CreateAt = DateTimeOffset.UtcNow;
                        entity.CreatedBy = currentUser?.UserId;
                        entity.IsDelete = false;
                    break;

                case EntityState.Modified:
                        entity.UpdateAt = DateTimeOffset.UtcNow;
                        entity.UpdatedBy = currentUser?.UserId;
                    break;

                case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entity.IsDelete = true;
                        entity.UpdateAt = DateTimeOffset.UtcNow;
                        entity.UpdatedBy = currentUser?.UserId ;
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }


    private static LambdaExpression GetIsDeletedRestriction(Type clrType)
    {
        var parameter = Expression.Parameter(clrType, "e");
        var property = Expression.Property(parameter, nameof(IBaseDomain.IsDelete));
        var condition = Expression.Equal(property, Expression.Constant(false));
        return Expression.Lambda(condition, parameter);
    }
    public DbSet<TaskItem> TaskItems { get; set; } = default!;
    public DbSet<Team> Teams { get; set; } = default!;
}
