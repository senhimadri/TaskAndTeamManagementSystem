using Microsoft.AspNetCore.SignalR.Client;

Console.Write("Enter your JWT token: ");

var token = Console.ReadLine();

var connection = new HubConnectionBuilder()
    .WithUrl("https://localhost:7260/hub/notifications", options =>
    {
        options.AccessTokenProvider = () => Task.FromResult(token);
    })
    .WithAutomaticReconnect()
    .Build();

connection.On<string>("ReceiveMessage", message =>
{
    Console.WriteLine($"Received message: {message}");
});

try
{
    await connection.StartAsync();
    Console.WriteLine("✅ Connected to SignalR hub.");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Connection failed: {ex.Message}");
    return;
}

Console.WriteLine("Listening for messages. Press any key to exit.");
Console.ReadKey();

await connection.StopAsync();
