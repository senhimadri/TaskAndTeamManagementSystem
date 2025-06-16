using MediatR;
using TaskAndTeamManagementSystem.Application.Dtos.TaskItemDtos;
using TaskAndTeamManagementSystem.Shared.Results;

namespace TaskAndTeamManagementSystem.Application.Features.TaskItems.Create;

public class CreateTaskItemCommand : IRequest<Result>
{
    public CreateTaskItemPayload Payload { get; set; } = default!;
}
