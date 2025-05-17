using TaskAndTeamManagementSystem.Application.Common.QueryFilters.EmployeeQueryBuilders;
using TaskAndTeamManagementSystem.Application.Dtos.EmployeeDtos;
using MediatR;

namespace TaskAndTeamManagementSystem.Application.Features.Employee.GetById;

public class GetByIdEmployeeRequest : IRequest<GetEmployeeByIdResponse?>
{
    public Guid Id { get; set; }
}
