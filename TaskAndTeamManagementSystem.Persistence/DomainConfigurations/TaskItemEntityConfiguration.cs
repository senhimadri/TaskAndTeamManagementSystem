using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskAndTeamManagementSystem.Domain;

namespace TaskAndTeamManagementSystem.Persistence.DomainConfigurations;

public class TaskItemEntityConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.HasOne(x => x.AssignedUser)
            .WithMany(x => x.AssignedTasks)
            .HasForeignKey(x => x.AssignedUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.CreatedByUser)
            .WithMany(x => x.CreatedTasks)
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
