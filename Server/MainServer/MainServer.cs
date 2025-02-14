using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using LauncherManagement;
using MyRPS;
using DataBase;

// Set up Dependency Injection
ServiceCollection services = new ServiceCollection();
services.AddSingleton<DatabaseHandler>();
services.AddSingleton<IRpsCommandHandler, RPS>();
services.AddSingleton<LauncherListener>();
services.AddSingleton<LauncherPoller>();
services.AddSingleton<TcpServer>();

using var serviceProvider = services.BuildServiceProvider();
LauncherListener listener = serviceProvider.GetRequiredService<LauncherListener>();
LauncherPoller poller = serviceProvider.GetRequiredService<LauncherPoller>();

Console.WriteLine("🔄 System initialized. Starting launcher polling...");

// Start monitoring
poller.StartPolling();


//start server
// Initialize any dependencies (like DatabaseHandler, RPS, etc.)
RPS rps = serviceProvider.GetRequiredService<IRpsCommandHandler>();
TcpServer server = serviceProvider.GetRequiredService<TcpServer>();

// Start the server asynchronously
await server.StartAsync();

//through server they can register new launcher?