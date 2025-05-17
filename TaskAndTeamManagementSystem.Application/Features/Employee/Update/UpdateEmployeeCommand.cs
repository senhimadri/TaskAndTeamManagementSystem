using TaskAndTeamManagementSystem.Application.Dtos.EmployeeDtos;
using MediatR;
using TaskAndTeamManagementSystem.Application.Helpers.Results;

namespace TaskAndTeamManagementSystem.Application.Features.Employee.Update;

public class UpdateEmployeeCommand : IRequest<Result>
{
    public Guid Id { get; set; }
    public UpdateEmployeePayload Payload { get; set; } = default!;
}
