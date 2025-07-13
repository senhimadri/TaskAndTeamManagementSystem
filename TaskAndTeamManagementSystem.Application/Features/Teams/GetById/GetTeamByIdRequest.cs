using MediatR;
using TaskAndTeamManagementSystem.Application.Dtos.TeamDtos;

namespace TaskAndTeamManagementSystem.Application.Features.Teams.GetById;

public class GetTeamByIdRequest : IRequest<GetTeamByIdResponse?>
{
    public int Id { get; set; }
}