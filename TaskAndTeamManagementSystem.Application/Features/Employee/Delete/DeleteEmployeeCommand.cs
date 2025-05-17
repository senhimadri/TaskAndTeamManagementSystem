using MediatR;
using TaskAndTeamManagementSystem.Application.Helpers.Results;

namespace TaskAndTeamManagementSystem.Application.Features.Employee.Delete;

public class DeleteEmployeeCommand : IRequest<Result>
{
    public Guid Id { get; set; }
}
