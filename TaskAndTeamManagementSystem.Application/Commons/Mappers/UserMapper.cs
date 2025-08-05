using TaskAndTeamManagementSystem.Application.Dtos.UserDtos;
using TaskAndTeamManagementSystem.Domain.Identities;

namespace TaskAndTeamManagementSystem.Application.Commons.Mappers;

public static class UserMapper
{
    public static ApplicationUser ToEntity(this CreateUserPayload dto)
    {
        return new ApplicationUser
        {
            Name = dto.Name,
            Email = dto.Email,
            UserName = dto.Email,
            PhoneNumber = dto.PhoneNumber
        };
    }

    public static void MapToEntity(this UpdateUserPayload dto, ApplicationUser entity)
    {
        entity.Name = dto.Name;
        entity.Email = dto.Email;
        entity.UserName = dto.Email;
        entity.PhoneNumber = dto.PhoneNumber;
    }

    public static GetUserByIdResponse ToGetByIdResponse(this ApplicationUser entity, IList<string> roles)
    {
        return new GetUserByIdResponse(
            entity.Id,
            entity.Name,
            entity.Email ?? string.Empty,
            entity.PhoneNumber,
            roles.ToList()
        );
    }

    public static IQueryable<GetUserListResponse> ToListResponse(IQueryable<ApplicationUser> entity)
    {
        return entity.Select(u => new GetUserListResponse(
            u.Id,
            u.Name,
            u.Email ?? string.Empty,
            u.PhoneNumber
        ));
    }

    public static IQueryable<GetUserListResponse> ToGetListResponse(this IQueryable<ApplicationUser> query)
    {
        return query.Select(u => new GetUserListResponse(
            u.Id,
            u.Name,
            u.Email ?? string.Empty,
            u.PhoneNumber
        ));
    }
}