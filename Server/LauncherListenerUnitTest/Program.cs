using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using LauncherManagement;
using MyRPS;
using DataBase;

// Set up Dependency Injection
ServiceCollection services = new ServiceCollection();
services.AddSingleton<MissileDbContext>();
services.AddSingleton<DatabaseHandler>();
services.AddSingleton<IRpsCommandHandler, RPS>();
services.AddSingleton<LauncherListener>();
services.AddSingleton<LauncherPoller>();

using var serviceProvider = services.BuildServiceProvider();
LauncherListener launcherListener = serviceProvider.GetRequiredService<LauncherListener>();
LauncherPoller poller = serviceProvider.GetRequiredService<LauncherPoller>();
DatabaseHandler dbHandler = serviceProvider.GetRequiredService<DatabaseHandler>();

Console.WriteLine("🔄 System initialized. Starting launcher polling...");

// Start monitoring
poller.StartPolling();

// Register a new launcher
poller.RemoveLauncher("L001");
poller.RemoveLauncher("L002");
await Task.Delay(500);

Launcher launcher = new Launcher("L001", "Base B", "Type-Y", dbHandler);
launcher.RegisterNewLauncher();
launcherListener.RegisterLauncher(launcher); ////
await Task.Delay(1000);
Launcher launcher2 = new Launcher("L002", "Base AB", "Type-X", dbHandler);
launcher2.RegisterNewLauncher();
launcherListener.RegisterLauncher(launcher2);
await Task.Delay(1000);

// Simulate a malfunction
Console.WriteLine($"🚨 Malfunction from Launcher {launcher.Code}!");
launcher.AlertMalfunction();
await Task.Delay(500);

Console.WriteLine($"🚨 Malfunction from Launcher {launcher2.Code}!");
launcher2.AlertMalfunction();
await Task.Delay(500);

// Stop polling and cleanup
Console.WriteLine("🛑 Stopping polling...");
poller.StopPolling();

Console.WriteLine("✅ Test completed. Check console logs for expected results.");
