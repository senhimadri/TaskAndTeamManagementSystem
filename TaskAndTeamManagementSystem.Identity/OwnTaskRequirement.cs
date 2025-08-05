using Microsoft.AspNetCore.Authorization;
using TaskAndTeamManagementSystem.Application.Contracts.Identities.IRepositories;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences;

namespace TaskAndTeamManagementSystem.Identity;

public class OwnTaskRequirement : IAuthorizationRequirement;
public class OwnTaskAuthorizationHandler(ICurrentUserService currentUser, IUnitOfWork unitOfWork) : AuthorizationHandler<OwnTaskRequirement, long>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnTaskRequirement requirement, long id)
    {
        var roles = currentUser.GetRoles();

        if (roles is null || !roles.Any())
        {
            context.Fail();
            return;
        }

        if (roles.Contains("Admin") || roles.Contains("Manager"))
        {
            context.Succeed(requirement);
            return;
        }

        var userId = currentUser.UserId;

        var isOwnTask = await unitOfWork.TaskItemRepository.IsAnyAsync(x => x.Id == id && x.AssignedUserId == userId);

        if (!isOwnTask)
        {
            context.Fail();
            return;
        }
        context.Succeed(requirement);
    }
}


public class OwnUserRequirement : IAuthorizationRequirement;

public class OwnUserAuthorizationHandler(ICurrentUserService currentUser) : AuthorizationHandler<OwnUserRequirement, Guid>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnUserRequirement requirement, Guid id)
    {
        var roles = currentUser.GetRoles();
        if (roles is null || !roles.Any())
        {
            context.Fail();
            return Task.CompletedTask;
        }

        if (roles.Contains("Admin") || roles.Contains("Manager"))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        var userId = currentUser.UserId;
        if (userId != id)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}
