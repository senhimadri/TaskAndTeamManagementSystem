using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TaskAndTeamManagementSystem.Application.Dtos.TaskItemDtos;

namespace TaskAndTeamManagementSystem.IntegrationTests.Controllers;

public class TaskItemControllerTests : IAsyncLifetime
{
    private readonly IntegrationTestFactory _factory;

    public TaskItemControllerTests()
    {
        _factory = new IntegrationTestFactory();
    }

    [Fact]
    public async Task Create_ShouldCreateTaskItemWithValidData()
    {
        var _admincClient = _factory.CreateAuthenticatedClient("Admin");
        using var dbContext = _factory.CreateDbContext();
        var assignedUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == "employee@demo.com");

        if (assignedUser == null)
        {
            throw new InvalidOperationException("Assigned user not found in the test database.");
        }

        var payload = new CreateTaskItemPayload(
                            Title: "New Test Task",
                            Description: "Test Description",
                            Status: 1,
                            DueDate: DateTime.Now.AddDays(1),
                            AssignedUserId : assignedUser.Id,
                            TeamId : 1);

        var response = await _admincClient.PostAsJsonAsync("/api/taskitem", payload);

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task ProtectedEndpoint_ShouldReturnUnauthorized_WhenTokenIsInvalid()
    {
        var _normalClient = _factory.CreateAuthenticatedClient("Employee");

        var payload = new CreateTaskItemPayload(
                    Title: "New Test Task",
                    Description: "Test Description",
                    Status: 1,
                    DueDate: DateTime.Now.AddDays(1),
                    AssignedUserId: Guid.Empty,
                    TeamId: 1);

        var response = await _normalClient.PostAsJsonAsync("/api/taskitem", payload);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    public async Task InitializeAsync()
    {
        await _factory.InitializeAsync();
        

    }
    public Task DisposeAsync() => _factory.DisposeAsync();
}
