using MediatR;
using TaskAndTeamManagementSystem.Application.Dtos.UserDtos;

namespace TaskAndTeamManagementSystem.Application.Features.Users.GetById;

public class GetUserByIdRequest : IRequest<GetUserByIdResponse?>
{
    public Guid Id { get; set; }
}
