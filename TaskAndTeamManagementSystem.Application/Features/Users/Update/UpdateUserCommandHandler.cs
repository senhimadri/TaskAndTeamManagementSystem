using MediatR;
using TaskAndTeamManagementSystem.Application.Commons.Mappers;
using TaskAndTeamManagementSystem.Application.Contracts.Identities;
using TaskAndTeamManagementSystem.Application.Dtos.UserDtos.Validator;
using TaskAndTeamManagementSystem.Application.Extensions;
using TaskAndTeamManagementSystem.Shared.Results;

namespace TaskAndTeamManagementSystem.Application.Features.Users.Update;

public class UpdateUserCommandHandler(IIdentityUnitOfWork unitOfWork) : IRequestHandler<UpdateUserCommand, Result>
{
    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await new UpdateUserPayloadValidator().ValidateAsync(request.Payload, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorList();

        var user = await unitOfWork.UserManager.FindByIdAsync(request.Id.ToString());
        if (user is null)
            return Errors.UserNotFound;

        request.Payload.MapToEntity(user);

        var result = await unitOfWork.UserManager.UpdateAsync(user);

        if (!result.Succeeded)
            return Errors.NewError(400, string.Join("; ", result.Errors.Select(e => e.Description)));

        var currentRoles = await unitOfWork.UserManager.GetRolesAsync(user);

        await unitOfWork.UserManager.RemoveFromRolesAsync(user, currentRoles);
        if (request.Payload.Roles is { Count: > 0 })
            await unitOfWork.UserManager.AddToRolesAsync(user, request.Payload.Roles);

        await unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}