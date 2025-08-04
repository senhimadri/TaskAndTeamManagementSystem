using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskAndTeamManagementSystem.Application.Commons.Mappers;
using TaskAndTeamManagementSystem.Application.Contracts.Identities;
using TaskAndTeamManagementSystem.Application.Dtos.UserDtos;

namespace TaskAndTeamManagementSystem.Application.Features.Users.GetById;

public class GetUserByIdRequestHandler(IIdentityUnitOfWork unitOfWork) : IRequestHandler<GetUserByIdRequest, GetUserByIdResponse?>
{
    public async Task<GetUserByIdResponse?> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.UserManager.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (user is null)
            return null;

        var roles = await unitOfWork.UserManager.GetRolesAsync(user);

        return user.ToGetByIdResponse(roles);
    }
}