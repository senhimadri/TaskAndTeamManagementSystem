using MassTransit;
using TaskAndTeamManagementSystem.Contracts;

namespace TaskAndTeamManagementSystem.MessageQueueClient;

public class TaskItemConsumer : IConsumer<CreateTaskItemEvent>, IConsumer<UpdateTaskItemEvent>, IConsumer<DeleteTaskItemEvent>
{
    public Task Consume(ConsumeContext<CreateTaskItemEvent> context)
    {
        Console.WriteLine($"Received CreateTaskItemEvent: {context.Message.Id}, Title: {context.Message.Title}, AssignedUserId: {context.Message.AssignedUserId}");

        return Task.CompletedTask;
    }
    public Task Consume(ConsumeContext<UpdateTaskItemEvent> context)
    {
        Console.WriteLine($"Received UpdateTaskItemEvent: {context.Message.Id}, Title: {context.Message.Title}, AssignedUserId: {context.Message.AssignedUserId}");
        return Task.CompletedTask;
    }
    public Task Consume(ConsumeContext<DeleteTaskItemEvent> context)
    {
        Console.WriteLine($"Received DeleteTaskItemEvent: {context.Message.Id}");
        return Task.CompletedTask;
    }
}