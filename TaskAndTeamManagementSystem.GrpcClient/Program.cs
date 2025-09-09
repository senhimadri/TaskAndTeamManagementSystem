using Grpc.Core;
using Grpc.Net.Client;
using TaskAndTeamManagementSystem.Contracts.Protos;

Console.Write("Enter your JWT token: ");
var token = Console.ReadLine();


using var channel = GrpcChannel.ForAddress("https://localhost:7260");
var client = new TaskItemService.TaskItemServiceClient(channel);

var headers = new Metadata
        {
            { "Authorization", $"Bearer {token}" }
        };


try
{
    var response = await client.GetTaskItemAsync(
        new GetTaskItemRequest { Id = 6 },
        headers
    );

    Console.WriteLine($"✅ Task Found: {response.Title} (ID: {response.Id})");
}
catch (RpcException ex)
{
    Console.WriteLine($"❌ gRPC Error: {ex.StatusCode} - {ex.Message}");
}
