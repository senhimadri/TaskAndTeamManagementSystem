using Moq;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences.IRepositories;
using TaskAndTeamManagementSystem.Domain;
using System.Linq.Expressions;

namespace TaskAndTeamManagementSystem.UnitTest.TestFixtures.Mocks;

internal class EmployeeRepositoryMock
{
    public readonly Mock<IEmployeeRepository> _employeeMock;


    public EmployeeRepositoryMock()
    {
        _employeeMock = new Mock<IEmployeeRepository>(MockBehavior.Strict);
    }


    public void SetupGetEmployeesWithDetails(IQueryable<Employee> employees)
    {
        _employeeMock.Setup(repo => repo.GetEmployeesWithDetails(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(employees);
    }
}
