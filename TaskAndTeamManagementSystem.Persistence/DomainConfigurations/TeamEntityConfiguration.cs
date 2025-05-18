using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskAndTeamManagementSystem.Domain;

namespace TaskAndTeamManagementSystem.Persistence.DomainConfigurations;

internal class TeamEntityConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.HasData(
            new Team
            {
                Id = 1,
                Name = "Test Team",
                Description = "This is a test team.",
                CreateAt = new DateTimeOffset(2025, 01, 01, 0, 0, 0, TimeSpan.Zero),
                IsDelete = false,
            });
    }
}
    
