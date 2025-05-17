using TaskAndTeamManagementSystem.Application.Contracts.Persistences;
using MediatR;
using TaskAndTeamManagementSystem.Application.Helpers.Results;

namespace TaskAndTeamManagementSystem.Application.Features.Employee.Delete;

public class DeleteEmployeeCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteEmployeeCommand,Result>
{
    public async Task<Result> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await  unitOfWork.EmployeeRepository.GetByIdAsync(request.Id);

        if (employee is null)
            return Errors.EmployeeNotFound;

        employee.IsDelete = true;

        await unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}