namespace TaskAndTeamManagementSystem.Application.Dtos.TeamDtos;

public record CreateTeamPayload(string Name, string? Description) : ITeamDto;

public record UpdateTeamPayload(string Name, string? Description) : ITeamDto;

public record GetTeamByIdResponse(int Id, string Name, string? Description);

public record GetTeamsListResponse(int Id, string Name, string? Description);

public record TeamPaginationQuery(string? SearchText, int PageNo, int PageSize, bool IsAscending);