using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TaskAndTeamManagementSystem.Application.Dtos.TaskItemDtos;
using Xunit;

namespace TaskAndTeamManagementSystem.IntegrationTests.Controllers;

public class TaskItemControllerTests : IAsyncLifetime
{
    private readonly IntegrationTestFactory _factory;
    private readonly HttpClient _client;

    public TaskItemControllerTests()
    {
        _factory = new IntegrationTestFactory();
        _client = _factory.CreateClient();

        var token = _factory.GenerateJwtToken("admin@demo.com", "Admin");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    [Fact]
    public async Task Create_ShouldCreateTaskItemWithValidData()
    {
        using var dbContext = _factory.CreateDbContext();
        var adminUser = await dbContext.Users.FirstAsync(u => u.Email == "employee-01@demo.com");

        var payload = new CreateTaskItemPayload(
                            Title: "New Test Task",
                            Description: "Test Description",
                            Status: 1,
                            DueDate: DateTime.Now.AddDays(1),
                            AssignedUserId : adminUser.Id,
                            TeamId : 1);

        var response = await _client.PostAsJsonAsync("/api/taskitem", payload);

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    public Task InitializeAsync() => _factory.InitializeAsync();
    public Task DisposeAsync() => _factory.DisposeAsync();
}
