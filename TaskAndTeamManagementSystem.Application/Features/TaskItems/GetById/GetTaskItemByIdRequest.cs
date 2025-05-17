using MediatR;
using TaskAndTeamManagementSystem.Application.Dtos.TaskItemDtos;

namespace TaskAndTeamManagementSystem.Application.Features.TaskItems.GetById;

public class GetTaskItemByIdRequest : IRequest<GetTaskItemByIdResponse>
{
    public long Id { get; set; }
}

internal class GetTaskItemByIdRequestHandler : IRequestHandler<GetTaskItemByIdRequest, GetTaskItemByIdResponse>
{
    public Task<GetTaskItemByIdResponse> Handle(GetTaskItemByIdRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
