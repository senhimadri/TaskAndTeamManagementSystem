using TaskAndTeamManagementSystem.Application.Contracts.Persistences;
using TaskAndTeamManagementSystem.Application.Dtos.EmployeeDtos.Validators;
using TaskAndTeamManagementSystem.Application.Common.Mappers;
using FluentValidation;
using MediatR;
using TaskAndTeamManagementSystem.Application.Helpers.Results;
using TaskAndTeamManagementSystem.Application.Helpers.Extensions;

namespace TaskAndTeamManagementSystem.Application.Features.Employee.Update;

public class UpdateEmployeeCommandHandler(IUnitOfWork unitofWork) : IRequestHandler<UpdateEmployeeCommand, Result>
{
    private readonly IUnitOfWork _unitofWork = unitofWork;

    public async Task<Result> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var validationResult = new UpdateEmployeePayloadValidator().Validate(request.Payload);

        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorList();

        var employee = await _unitofWork.EmployeeRepository.GetByIdAsync(request.Id);

        if (employee is null)
            return Errors.EmployeeNotFound;

        employee.UpdateEntity(request.Payload);

        _unitofWork.EmployeeRepository.Update(employee);

        await _unitofWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
