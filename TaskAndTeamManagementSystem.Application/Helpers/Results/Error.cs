namespace TaskAndTeamManagementSystem.Application.Helpers.Results;

public sealed record Error(int Code, string? Message = null)
{
    public static readonly Error None = new(200, string.Empty);
}
