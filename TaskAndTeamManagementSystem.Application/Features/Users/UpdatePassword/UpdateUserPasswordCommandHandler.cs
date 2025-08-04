using MediatR;
using TaskAndTeamManagementSystem.Application.Contracts.Identities;
using TaskAndTeamManagementSystem.Application.Dtos.UserDtos.Validator;
using TaskAndTeamManagementSystem.Application.Extensions;
using TaskAndTeamManagementSystem.Shared.Results;

namespace TaskAndTeamManagementSystem.Application.Features.Users.UpdatePassword;

public class UpdateUserPasswordCommandHandler(IIdentityUnitOfWork unitOfWork) : IRequestHandler<UpdateUserPasswordCommand, Result>
{
    public async Task<Result> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await new UpdateUserPasswordPayloadValidator().ValidateAsync(request.Payload, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorList();


        var user = await unitOfWork.UserManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
            return Errors.UserNotFound;

        var result = await unitOfWork.UserManager.ChangePasswordAsync(user, request.Payload.CurrentPassword, request.Payload.NewPassword);
        if (!result.Succeeded)
            return Errors.NewError(400, string.Join("; ", result.Errors.Select(e => e.Description)));

        await unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}