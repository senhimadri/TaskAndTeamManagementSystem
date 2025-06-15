namespace TaskAndTeamManagementSystem.Application.Dtos.AuthDtos;

public class GoogleUserInfoDto
{
    public string Sub { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Picture { get; set; } = default!;
}
