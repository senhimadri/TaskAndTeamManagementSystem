using MediatR;
using TaskAndTeamManagementSystem.Application.Contracts.Identities;
using TaskAndTeamManagementSystem.Application.Contracts.Identities.IRepositories;
using TaskAndTeamManagementSystem.Application.Dtos.AuthDto;

namespace TaskAndTeamManagementSystem.Application.Features.Auths.Login;

internal class LoginCommandHandler(IIdentityUnitOfWork unitOfWork, ITokenUtils tokenUtils) : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.UserManager.FindByNameAsync(request.Payload.UserName);

        if (user is null)
            throw new UnauthorizedAccessException("Invalid credentials");

        var result = await unitOfWork.SignInManager.CheckPasswordSignInAsync(user, request.Payload.Password, false);
        if (!result.Succeeded)
            throw new UnauthorizedAccessException("Invalid credentials");

        var roles = await unitOfWork.UserManager.GetRolesAsync(user);

        var accessToken = tokenUtils.GenerateAccessToken(user, roles);
        var refreshToken = tokenUtils.GenerateRefreshToken(user.Id);

        await unitOfWork.SaveChangesAsync();

        return new LoginResponse(accessToken, refreshToken);
    }
}
