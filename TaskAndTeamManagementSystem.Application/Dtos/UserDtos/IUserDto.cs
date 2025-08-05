namespace TaskAndTeamManagementSystem.Application.Dtos.UserDtos;

public interface IUserDto
{
    string Name { get; }
    string Email { get; }
    string? PhoneNumber { get; }
}
