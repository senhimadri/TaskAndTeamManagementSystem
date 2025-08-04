using MediatR;
using TaskAndTeamManagementSystem.Application.Commons.Mappers;
using TaskAndTeamManagementSystem.Application.Contracts.Identities;
using TaskAndTeamManagementSystem.Application.Dtos.UserDtos.Validator;
using TaskAndTeamManagementSystem.Application.Extensions;
using TaskAndTeamManagementSystem.Domain.Identities;
using TaskAndTeamManagementSystem.Shared.Results;

namespace TaskAndTeamManagementSystem.Application.Features.Users.Create;

public class CreateUserCommandHandler(IIdentityUnitOfWork unitOfWork) : IRequestHandler<CreateUserCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await new CreateUserPayloadValidator().ValidateAsync(request.Payload, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorList();

        var user = request.Payload.ToEntity();

        var result = await unitOfWork.UserManager.CreateAsync(user, request.Payload.Password);
        if (!result.Succeeded)
            return Errors.NewError(400, string.Join("; ", result.Errors.Select(e => e.Description)));

        if (request.Payload.Roles is { Count: > 0 })
            await unitOfWork.UserManager.AddToRolesAsync(user, request.Payload.Roles);

        await unitOfWork.SaveChangesAsync();
        return Result<Guid>.Success(user.Id);
    }
}