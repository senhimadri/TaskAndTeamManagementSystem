using TaskAndTeamManagementSystem.Application.Dtos.EmployeeDtos;
using MediatR;
using TaskAndTeamManagementSystem.Application.Helpers.Results;

namespace TaskAndTeamManagementSystem.Application.Features.Employee.Create;

public class CreateEmployeeCommand : IRequest<Result>
{
    public CreateEmployeePayload Payload { get; set; } = default!;
}
