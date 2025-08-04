using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using TaskAndTeamManagementSystem.Application.Contracts.Identities.IRepositories;
using TaskAndTeamManagementSystem.Domain.Identities;

namespace TaskAndTeamManagementSystem.Identity.Repositories;

internal class TokenUtils(IOptions<JwtSettings> options) : ITokenUtils
{
    public string GenerateAccessToken(ApplicationUser user, IList<string>? roles)
    {
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(options.Value.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
       {
           new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
           new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
           new(ClaimTypes.Name, user.UserName ?? string.Empty),
           new(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        if (roles != null)
        {
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
        }

        var token = new JwtSecurityToken(
            issuer: options.Value.Issuer,
            audience: options.Value.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(options.Value.AccessTokenExpirationMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken(Guid userId)
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);
        var userIdBytes = userId.ToByteArray();

        var combinedBytes = randomBytes.Concat(userIdBytes).ToArray();
        var hashedBytes = SHA256.HashData(combinedBytes);
        return Convert.ToBase64String(hashedBytes);
    }
}
