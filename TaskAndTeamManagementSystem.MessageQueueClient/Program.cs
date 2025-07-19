using TaskAndTeamManagementSystem.MessageQueueClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransitWithRabbitMQConfiguration();

var app = builder.Build();

Console.WriteLine("Message Queue connection established. Waiting for message..");

app.Run();
