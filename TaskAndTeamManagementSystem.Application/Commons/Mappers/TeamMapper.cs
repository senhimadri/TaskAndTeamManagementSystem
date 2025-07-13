using TaskAndTeamManagementSystem.Application.Dtos.TeamDtos;
using TaskAndTeamManagementSystem.Domain;

namespace TaskAndTeamManagementSystem.Application.Commons.Mappers;

public static class TeamMapper
{
    public static Team ToEntity(this CreateTeamPayload dto)
        => new Team { Name = dto.Name, Description = dto.Description };

    public static void MapToEntity(this UpdateTeamPayload dto, Team entity)
    {
        entity.Name = dto.Name;
        entity.Description = dto.Description;
    }

    public static GetTeamByIdResponse ToGetByIdDto(Team entity)
        => new(entity.Id, entity.Name, entity.Description);

    public static GetTeamsListResponse ToListDto(Team entity)
        => new(entity.Id, entity.Name, entity.Description);

    public static IQueryable<GetTeamByIdResponse> ToGetByIdResponse(this IQueryable<Team> query)
        => query.Select(entity => new GetTeamByIdResponse(entity.Id, entity.Name, entity.Description));

    public static IQueryable<GetTeamsListResponse> ToGetListResponse(this IQueryable<Team> query)
        => query.Select(entity => new GetTeamsListResponse(entity.Id, entity.Name, entity.Description));
}
