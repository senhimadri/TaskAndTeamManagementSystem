namespace TaskAndTeamManagementSystem.Application.Dtos.UserDtos;

public record CreateUserPayload(string Name, string Email, string? PhoneNumber, string Password, List<string> Roles) : IUserDto;
public record UpdateUserPayload(string Name , string Email, string? PhoneNumber, List<string> Roles) : IUserDto;
public record UpdateUserPasswordPayload(string CurrentPassword, string NewPassword);
public record GetUserListResponse(Guid Id, string Name , string Email, string? PhoneNumber);
public record GetUserByIdResponse(Guid Id, string Name  , string Email, string? PhoneNumber , List<string> Roles);
public record UserPaginationQuery(string? SearchText, int PageNo, int PageSize, bool IsAscending);

