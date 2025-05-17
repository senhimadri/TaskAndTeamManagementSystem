using Azure.Core;
using Moq;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences.IRepositories;
using TaskAndTeamManagementSystem.Application.Dtos.EmployeeDtos;
using TaskAndTeamManagementSystem.Application.Dtos.EmployeeDtos.Validators;
using TaskAndTeamManagementSystem.Application.Features.Employee.Create;
using TaskAndTeamManagementSystem.Domain;
using TaskAndTeamManagementSystem.Persistence;
using TaskAndTeamManagementSystem.UnitTest.Shareds;

namespace TaskAndTeamManagementSystem.UnitTest.Services;

public class CreateEmployeeCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;

    public CreateEmployeeCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _employeeRepositoryMock = new Mock<IEmployeeRepository>();
        _unitOfWorkMock.Setup(u => u.EmployeeRepository).Returns(_employeeRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidPayload_ReturnsSuccessResult()
    {
        var payload = new CreateEmployeePayload(
           FirstName: "Himadri",
           LastName: "Apu",
           DateOfBirth: new DateTimeOffset(new DateTime(1997, 1, 1, 0, 0, 0, DateTimeKind.Utc), TimeSpan.Zero),
           DepartmentId: 1,
           DesignationId: 1
        ); 

        var command = new CreateEmployeeCommand { Payload = payload };

        _employeeRepositoryMock.Setup(r => r.Add(It.IsAny<Employee>()));

        _unitOfWorkMock.Setup(u=> u.SaveChangesAsync(It.IsAny<CancellationToken>()));

        var handler = new CreateEmployeeCommandHandler(_unitOfWorkMock.Object);


        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Null(result.ValidationErrors);

        _employeeRepositoryMock.Verify(r => r.Add(It.IsAny<Employee>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidDepartmentId_ReturnsValidationFailed()
    {
        var payload = new CreateEmployeePayload(
           FirstName: "Himadri",
           LastName: "Apu",
           DateOfBirth: new DateTimeOffset(new DateTime(1997, 1, 1, 0, 0, 0, DateTimeKind.Utc), TimeSpan.Zero),
           DepartmentId: 0,
           DesignationId: 1
        );

        var command = new CreateEmployeeCommand { Payload = payload };

        var handler = new CreateEmployeeCommandHandler(_unitOfWorkMock.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.True(result.IsValidationFailure);
        Assert.NotNull(result.ValidationErrors);

        Assert.Contains("DepartmentId", result.ValidationErrors.Keys);
    }
}
