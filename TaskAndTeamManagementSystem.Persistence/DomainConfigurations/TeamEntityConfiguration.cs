using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Globalization;
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
                CreateAt = DateTimeOffset.Parse("2025-01-01T00:00:00+00:00", CultureInfo.InvariantCulture),
                IsDelete = false,
            });
    }
}

