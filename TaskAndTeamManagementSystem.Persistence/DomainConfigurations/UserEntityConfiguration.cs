using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskAndTeamManagementSystem.Domain;

namespace TaskAndTeamManagementSystem.Persistence.DomainConfigurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasData(
            new User
            {
                Id = new Guid("11111111-1111-1111-1111-111111111111"),
                Name = "Test Admin",
                Email = "test.admin@tms.com",
                CreateAt = new DateTimeOffset(2025, 01, 01, 0, 0, 0, TimeSpan.Zero),
                IsDelete = false
            },
            new User
            {
                Id = new Guid("22222222-2222-2222-2222-222222222222"),
                Name = "Test Manager",
                Email = "test.manager@tms.com",
                CreateAt = new DateTimeOffset(2025, 01, 01, 0, 0, 0, TimeSpan.Zero),
                IsDelete = false
            },
            new User
            {
                Id = new Guid("33333333-3333-3333-3333-333333333333"),
                Name = "Test Employee",
                Email = "test.employee@tms.com",
                CreateAt = new DateTimeOffset(2025, 01, 01, 0, 0, 0, TimeSpan.Zero),
                IsDelete = false
            });
    }
}
