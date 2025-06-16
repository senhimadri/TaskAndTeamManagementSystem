
namespace TaskAndTeamManagementSystem.Application.Dtos.AuthDtos;

public record CreateUserPayload(string Name, string Email, string UserName, string Password) : IUserRegistrationDto, IPasswordDto;
public record UpdateUserPayload(string Name, string Email, string UserName) : IUserRegistrationDto;
public record UpdatePasswordPayload(string Password, string Email, string UserName) : IPasswordDto;
public record GetUserIdResponse(string Name, string Email, string UserName);
public record GetEmployeesListResponse(Guid Id, string Name, string Email, string UserName);
public record UserPaginationQuery(string? SearchText, int PageNo, int PageSize);
