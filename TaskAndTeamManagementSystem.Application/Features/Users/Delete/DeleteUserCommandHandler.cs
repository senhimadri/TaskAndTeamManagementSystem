using MediatR;
using TaskAndTeamManagementSystem.Application.Contracts.Identities;
using TaskAndTeamManagementSystem.Shared.Results;

namespace TaskAndTeamManagementSystem.Application.Features.Users.Delete;

public class DeleteUserCommandHandler(IIdentityUnitOfWork unitOfWork) : IRequestHandler<DeleteUserCommand, Result>
{
    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.UserManager.FindByIdAsync(request.Id.ToString());
        if (user is null)
            return Errors.UserNotFound;

        var result = await unitOfWork.UserManager.DeleteAsync(user);
        if (!result.Succeeded)
            return Errors.NewError(400, string.Join("; ", result.Errors.Select(e => e.Description)));

        await unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}