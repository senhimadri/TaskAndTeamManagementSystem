using MediatR;
using System.Text.Json;
using TaskAndTeamManagementSystem.Application.Contracts.Identities;
using TaskAndTeamManagementSystem.Application.Contracts.Identities.IRepositories;
using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.HttpClients;
using TaskAndTeamManagementSystem.Application.Dtos.AuthDto;
using TaskAndTeamManagementSystem.Application.Dtos.AuthDtos;

namespace TaskAndTeamManagementSystem.Application.Features.Auths.GoogleLogin;

public class GoogleLoginCommandHandler(ITokenUtils _tokenUtils, IIdentityUnitOfWork _unitOfWork, IGoogleAuthService _httpClientFactory) : IRequestHandler<GoogleLoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
    {
        var googleUser = await _httpClientFactory.GetUserInfoAsync(request.Token, cancellationToken);

        if (googleUser == null)
            throw new UnauthorizedAccessException("Invalid Google token.");

        if (string.IsNullOrEmpty(googleUser.Email))
            throw new UnauthorizedAccessException("Failed to parse Google user.");

        var user = await _unitOfWork.UserManager.FindByEmailAsync(googleUser.Email);
        if (user == null)
        {
            user = new Domain.Identities.ApplicationUser
            {
                Name = googleUser.Name,
                UserName = googleUser.Email,
                Email = googleUser.Email,
                EmailConfirmed = true
            };
            await _unitOfWork.UserManager.CreateAsync(user);
        }

        var AccessToken = _tokenUtils.GenerateAccessToken(user, new List<string>() { "Guest" });
        var RefreshToken = _tokenUtils.GenerateRefreshToken(user.Id);

        return new LoginResponse(AccessToken, RefreshToken);
    }
}
