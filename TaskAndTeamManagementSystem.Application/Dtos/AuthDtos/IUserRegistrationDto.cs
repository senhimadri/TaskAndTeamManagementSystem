
namespace TaskAndTeamManagementSystem.Application.Dtos.AuthDtos;

public interface IUserRegistrationDto
{
    public string Name { get; }
    public string Email { get; }
    public string UserName { get; }
}

public interface IPasswordDto
{
    public string Password { get; }
}