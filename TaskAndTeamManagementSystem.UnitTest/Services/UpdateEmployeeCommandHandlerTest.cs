using Moq;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences.IRepositories;
using TaskAndTeamManagementSystem.Application.Dtos.EmployeeDtos;
using TaskAndTeamManagementSystem.Application.Features.Employee.Update;
using TaskAndTeamManagementSystem.Application.Helpers.Results;
using TaskAndTeamManagementSystem.Domain;

namespace TaskAndTeamManagementSystem.UnitTest.Services;

public class UpdateEmployeeCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock; 
    private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;

    public UpdateEmployeeCommandHandlerTest()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _employeeRepositoryMock = new Mock<IEmployeeRepository>();
        _unitOfWorkMock.Setup(u => u.EmployeeRepository).Returns(_employeeRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenUpdateIsValid()
    {
        var id = Guid.NewGuid();

        var payload = new UpdateEmployeePayload(
            FirstName: "Himadri",
            LastName: "Apu",
            DateOfBirth: new DateTimeOffset(new DateTime(1997, 1, 1, 0, 0, 0, DateTimeKind.Utc), TimeSpan.Zero),
            DepartmentId: 1,
            DesignationId: 1
        );

        var employee = new Employee
        {
            Id = id,
            FirstName = "OldFirstName",
            LastName = "OldLastName",
            DateOfBirth = new DateTimeOffset(new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc), TimeSpan.Zero),
            DepartmentId = 1,
            DesignationId = 1
        };

        _employeeRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(employee);

        var command = new UpdateEmployeeCommand
        {
            Id = id,
            Payload = payload
        };
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()));

        var handler = new UpdateEmployeeCommandHandler(_unitOfWorkMock.Object);
        var result = await handler.Handle(command, CancellationToken.None);


        Assert.True(result.IsSuccess);

        Assert.Null(result.ValidationErrors);

        _employeeRepositoryMock.Verify(r => r.Update(It.IsAny<Employee>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }


    [Fact]
    public async Task Handle_ShouldReturnEmployeeNotFound_WhenEmployeeDoesNotExist()
    {

        var id = Guid.NewGuid();

        var payload = new UpdateEmployeePayload(
            FirstName: "Himadri",
            LastName: "Apu",
            DateOfBirth: new DateTimeOffset(new DateTime(1997, 1, 1, 0, 0, 0, DateTimeKind.Utc), TimeSpan.Zero),
            DepartmentId: 1,
            DesignationId: 1
        );

        var command = new UpdateEmployeeCommand
        {
            Id = id,
            Payload = payload
        };
        _employeeRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Employee?)null);

        var handler = new UpdateEmployeeCommandHandler(_unitOfWorkMock.Object);
        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(Errors.EmployeeNotFound, result.Error);

        _employeeRepositoryMock.Verify(r => r.Update(It.IsAny<Employee>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }



}