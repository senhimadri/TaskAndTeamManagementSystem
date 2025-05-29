using MediatR;
using TaskAndTeamManagementSystem.Application.Contracts.Identities;
using TaskAndTeamManagementSystem.Application.Contracts.Identities.IRepositories;
using TaskAndTeamManagementSystem.Application.Dtos.LoginDtos;

namespace TaskAndTeamManagementSystem.Application.Features.Auths.RefreshToken;

public class RefreshTokenCommand : IRequest<LoginResponse>
{
    public string RefreshToken { get; set; } = string.Empty;
}

public class RefreshTokenCommandHandler(IIdentityUnitOfWork unitOfWork, ITokenUtils tokenUtils) : IRequestHandler<RefreshTokenCommand, LoginResponse>
{
    public Task<LoginResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException("Refresh token logic is not implemented yet. Please implement the logic to handle refresh tokens.");
    }
}
