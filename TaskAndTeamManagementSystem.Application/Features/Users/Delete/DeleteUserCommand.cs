using MediatR;
using TaskAndTeamManagementSystem.Shared.Results;

namespace TaskAndTeamManagementSystem.Application.Features.Users.Delete;

public class DeleteUserCommand : IRequest<Result>
{
    public Guid Id { get; set; }
}
