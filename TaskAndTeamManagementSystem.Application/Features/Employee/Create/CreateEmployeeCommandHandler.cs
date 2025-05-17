using MediatR;
using TaskAndTeamManagementSystem.Application.Common.Mappers;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences;
using TaskAndTeamManagementSystem.Application.Dtos.EmployeeDtos.Validators;
using TaskAndTeamManagementSystem.Application.Helpers.Extensions;
using TaskAndTeamManagementSystem.Application.Helpers.Results;

namespace TaskAndTeamManagementSystem.Application.Features.Employee.Create;

public class CreateEmployeeCommandHandler(IUnitOfWork unitofWork) : IRequestHandler<CreateEmployeeCommand, Result>
{
    private readonly IUnitOfWork _unitofWork = unitofWork;

    public async Task<Result> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await  new CreateEmployeePayloadValidator()
                               .ValidateAsync(request.Payload);

        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorList();

        var employee = request.Payload.ToEntity();

        _unitofWork.EmployeeRepository.Add(employee);

        await _unitofWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

