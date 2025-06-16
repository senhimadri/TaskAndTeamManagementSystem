using MediatR;
using TaskAndTeamManagementSystem.Application.Dtos.TaskItemDtos;
using TaskAndTeamManagementSystem.Shared.Results;


namespace TaskAndTeamManagementSystem.Application.Features.TaskItems.Update;

public class UpdateTaskItemCommand : IRequest<Result>
{
    public long Id { get; set; }
    public UpdateTaskItemPayload Payload { get; set; } = default!;
}
