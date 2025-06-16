namespace TaskAndTeamManagementSystem.Application.Dtos.CommonDtos;

public record GetPaginationResponse<T>(int PageNo, int PageSize, int TotalCount, List<T>? data);
