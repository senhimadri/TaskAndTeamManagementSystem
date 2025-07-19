using TaskAndTeamManagementSystem.Application.Dtos.TaskItemDtos;
using TaskAndTeamManagementSystem.Domain;
using TaskAndTeamManagementSystem.Shared.Extensions;

namespace TaskAndTeamManagementSystem.Application.Commons.Mappers;


public static class TaskItemMapper
{
    public static TaskItem ToEntity(this CreateTaskItemPayload dto, Guid createdByUserId)
    {
        return new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description ?? string.Empty,
            Status = (ActivityStatus)dto.Status,
            DueDate = dto.DueDate ?? DateTimeOffset.UtcNow,
            AssignedUserId = dto.AssignedUserId,
            CreatedByUserId = createdByUserId,
            TeamId = dto.TeamId
        };
    }

    public static void MapToEntity(this UpdateTaskItemPayload dto, TaskItem entity)
    {
        entity.Title = dto.Title;
        entity.Description = dto.Description ?? string.Empty;
        entity.Status = (ActivityStatus)dto.Status;
        entity.DueDate = dto.DueDate ?? DateTimeOffset.UtcNow;
        entity.AssignedUserId = dto.AssignedUserId;
        entity.TeamId = dto.TeamId;
    }

    public static GetTaskItemByIdResponse ToGetByIdDto(TaskItem entity)
    {
        return new GetTaskItemByIdResponse(
            Title: entity.Title,
            Description: string.IsNullOrWhiteSpace(entity.Description) ? null : entity.Description,
            Status: (int)entity.Status,
            StatusName: entity.Status.ToString(),
            DueDate: entity.DueDate,
            AssignedUserId: entity.AssignedUserId,
            AssignedUserName: entity.AssignedUser?.Name ?? "Unknown",
            CreatedById: entity.CreatedByUserId,
            CreatedByName: entity.CreatedByUser?.Name ?? "Unknown",
            TeamId: entity.TeamId,
            TeamName: entity.Team?.Name ?? "Unknown"
        );
    }

    public static GetTaskItemsListResponse ToListDto(TaskItem entity)
    {
        return new GetTaskItemsListResponse(
            Id: entity.Id,
            Title: entity.Title,
            Status: entity.Status.ToString(),
            AssignedUserName: entity.AssignedUser?.Name ?? "Unknown",
            CreatedByName: entity.CreatedByUser?.Name ?? "Unknown",
            TeamName: entity.Team?.Name ?? "Unknown"
        );
    }

    public static IQueryable<GetTaskItemByIdResponse> ToGetByIdResponse(this IQueryable<TaskItem> query)
    {
        return query.Select(entity => new GetTaskItemByIdResponse(
             entity.Title,
             entity.Description,
             (int)entity.Status,
             entity.Status.GetDisplayName(),
             entity.DueDate,
             entity.AssignedUserId,
             entity.AssignedUser.Name ?? "Unknown",
             entity.CreatedByUserId,
             entity.CreatedByUser.Name,
             entity.TeamId,
             entity.Team != null ? entity.Team.Name : null
        ));
    }

    public static IQueryable<GetTaskItemsListResponse> ToGetListResponse(this IQueryable<TaskItem> query)
    {
        return query.Select(entity => new GetTaskItemsListResponse(
             entity.Id,
             entity.Title,
             entity.Status == ActivityStatus.ToDo ? "To Do" : entity.Status == ActivityStatus.InProgress ? "In Progress" : "Done",
             entity.AssignedUser.Name,
             entity.CreatedByUser.Name,
             entity.Team != null ? entity.Team.Name : "Unknown"
        ));
    }
}
