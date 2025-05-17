using TaskAndTeamManagementSystem.Application.Common.Mappers;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences;
using TaskAndTeamManagementSystem.Application.Dtos.EmployeeDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace TaskAndTeamManagementSystem.Application.Features.Employee.GetById;

public class GetByIdEmployeeRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetByIdEmployeeRequest, GetEmployeeByIdResponse?>
{
    public async Task<GetEmployeeByIdResponse?> Handle(GetByIdEmployeeRequest request, CancellationToken cancellationToken)
    {
        var query = unitOfWork.EmployeeRepository.GetEmployeesWithDetails(x=>x.Id == request.Id);

        var employee = await query.ToGetByIdResponse().FirstOrDefaultAsync();

        return employee;
    }
}
