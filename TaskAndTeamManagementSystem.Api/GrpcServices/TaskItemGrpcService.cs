using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using TaskAndTeamManagementSystem.Application.Features.TaskItems.GetById;
using TaskAndTeamManagementSystem.Contracts.Protos;

namespace TaskAndTeamManagementSystem.Api.GrpcServices;

[Authorize(Policy = "Employee")]
public class TaskItemGrpcService(IMediator mediator) : TaskItemService.TaskItemServiceBase
{
    public override async Task<TaskItemResponse> GetTaskItem(GetTaskItemRequest request, ServerCallContext context)
    {
        var result = await mediator.Send(new GetTaskItemByIdRequest { Id = request.Id }, context.CancellationToken);

        if (result is null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Task item with ID {request.Id} not found."));

        return new TaskItemResponse
        {
            Id = request.Id,
            Title = result.Title,
            Description = result.Description ?? string.Empty,
            Status = result.Status,
            StatusName = result.StatusName,
            DueDate = result.DueDate.HasValue ? Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(result.DueDate.Value) : null,
            AssignedUserId = result.AssignedUserId.ToString(),
            AssignedUserName = result.AssignedUserName,
            CreatedById = result.CreatedById.ToString(),
            CreatedByName = result.CreatedByName,
            TeamId = result.TeamId,
            TeamName = result.TeamName ?? string.Empty
        };

    }
}
