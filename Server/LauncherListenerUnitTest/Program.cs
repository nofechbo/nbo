using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using LauncherManagement;
using mainServer;
using DataBase;

// ✅ 1. Set up Dependency Injection
var services = new ServiceCollection();
services.AddSingleton<DatabaseHandler>();
services.AddSingleton<IRpsCommandHandler, RPS>();
services.AddSingleton<LauncherListener>();
services.AddSingleton<LauncherPoller>();

using var serviceProvider = services.BuildServiceProvider();
var launcherListener = serviceProvider.GetRequiredService<LauncherListener>();
var poller = serviceProvider.GetRequiredService<LauncherPoller>();

Console.WriteLine("🔄 System initialized. Starting launcher polling...");

// ✅ 2. Start monitoring
poller.StartPolling();

// ✅ 3. Register a new launcher
var launcher = new Launcher("L002", "Base B", "Type-Y");
launcherListener.RegisterLauncher(launcher);
Console.WriteLine($"🆕 Launcher {launcher.Code} registered and being monitored.");

// ✅ 4. Allow time for polling to process
await Task.Delay(500);

// ✅ 5. Simulate a malfunction
Console.WriteLine($"🚨 Malfunction in Launcher {launcher.Code}!");
launcher.AlertMalfunction();

// ✅ 6. Allow system time to process the malfunction and send technician
await Task.Delay(500);

// ✅ 7. Stop polling and cleanup
Console.WriteLine("🛑 Stopping polling...");
poller.StopPolling();

Console.WriteLine("✅ Test completed. Check console logs for expected results.");
