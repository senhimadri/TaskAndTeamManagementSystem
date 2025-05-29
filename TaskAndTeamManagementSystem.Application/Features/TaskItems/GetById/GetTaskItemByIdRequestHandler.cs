using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskAndTeamManagementSystem.Application.Commons.Mappers;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences;
using TaskAndTeamManagementSystem.Application.Dtos.TaskItemDtos;

namespace TaskAndTeamManagementSystem.Application.Features.TaskItems.GetById;

internal class GetTaskItemByIdRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetTaskItemByIdRequest, GetTaskItemByIdResponse?>
{
    public async Task<GetTaskItemByIdResponse?> Handle(GetTaskItemByIdRequest request, CancellationToken cancellationToken)
    {
        var query = unitOfWork.TaskItemRepository.TaskItemDetails(x=>x.Id == request.Id);

        var taskItem = await query.ToGetByIdResponse().FirstOrDefaultAsync(cancellationToken);

        return taskItem;

    }
}
