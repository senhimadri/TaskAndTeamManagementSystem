using System.Net.Http;
using System.Text.Json;
using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.HttpClients;
using TaskAndTeamManagementSystem.Application.Dtos.AuthDtos;

namespace TaskAndTeamManagementSystem.Infrastructure.HttpClients;

internal class GoogleAuthService(IHttpClientFactory httpClientFactory) : IGoogleAuthService
{
    public async Task<GoogleUserInfoDto> GetUserInfoAsync(string accessToken, CancellationToken cancellationToken = default)
    {
        const string googleUserInfoEndpoint = "https://www.googleapis.com/oauth2/v3/userinfo";
        var httpClient = httpClientFactory.CreateClient();

        var response = await httpClient.GetAsync($"{googleUserInfoEndpoint}?access_token={accessToken}", cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new UnauthorizedAccessException("Invalid Google token.");

        var json = await response.Content.ReadAsStringAsync(cancellationToken);

        var googleUser = JsonSerializer.Deserialize<GoogleUserInfoDto>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (googleUser == null || string.IsNullOrEmpty(googleUser.Email))
            throw new UnauthorizedAccessException("Failed to parse Google user.");

        return googleUser;
    }
}
