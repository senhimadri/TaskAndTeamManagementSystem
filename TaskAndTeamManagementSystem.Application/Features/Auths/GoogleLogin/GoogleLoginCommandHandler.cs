using MediatR;
using System.Text.Json;
using TaskAndTeamManagementSystem.Application.Contracts.Identities;
using TaskAndTeamManagementSystem.Application.Contracts.Identities.IRepositories;
using TaskAndTeamManagementSystem.Application.Dtos.AuthDto;
using TaskAndTeamManagementSystem.Application.Dtos.AuthDtos;
using TaskAndTeamManagementSystem.Domain;

namespace TaskAndTeamManagementSystem.Application.Features.Auths.GoogleLogin;

public class GoogleLoginCommandHandler(ITokenUtils _tokenUtils, IIdentityUnitOfWork _unitOfWork, IHttpClientFactory _httpClientFactory) : IRequestHandler<GoogleLoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync(
            $"https://www.googleapis.com/oauth2/v3/userinfo?access_token={request.Token}");

        if (!response.IsSuccessStatusCode)
            throw new UnauthorizedAccessException("Invalid Google token.");

        var json = await response.Content.ReadAsStringAsync();
        var googleUser = JsonSerializer.Deserialize<GoogleUserInfoDto>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (googleUser == null || string.IsNullOrEmpty(googleUser.Email))
            throw new UnauthorizedAccessException("Failed to parse Google user.");

        var user = await _unitOfWork.UserManager.FindByEmailAsync(googleUser.Email);
        if (user == null)
        {
            user = new ApplicationUser
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
