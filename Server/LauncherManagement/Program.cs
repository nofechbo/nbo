

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using LauncherManagement;
using MyRPS;
using DataBase;

public class LauncherTest
{
    public static async Task Main(string[] args) {
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

        // Start monitoring asynchronously
        poller.StartPolling();

        poller.RemoveLauncher("L001");
        poller.RemoveLauncher("L002");
        await Task.Delay(500);  // Wait a bit for the system to settle

        // Register the first launcher
        Launcher launcher = new Launcher("L001", "Base B", "Type-Y", dbHandler);
        launcher.RegisterNewLauncher();
        launcherListener.RegisterLauncher(launcher);
        await Task.Delay(1000);  // Wait to ensure it's registered and the listener has it

        // Register the second launcher
        Launcher launcher2 = new Launcher("L002", "Base AB", "Type-X", dbHandler);
        launcher2.RegisterNewLauncher();
        launcherListener.RegisterLauncher(launcher2);
        await Task.Delay(1000);  // Allow time for the second launcher to be registered

        // Simulate a malfunction on the first launcher
        Console.WriteLine($"🚨 Malfunction from Launcher {launcher.Code}!");
        launcher.AlertMalfunction();
        await Task.Delay(500);  // Wait for the malfunction to be processed

        // Simulate a malfunction on the second launcher
        Console.WriteLine($"🚨 Malfunction from Launcher {launcher2.Code}!");
        launcher2.AlertMalfunction();
        await Task.Delay(500);  // Wait for the malfunction to be processed

        // Stop polling and cleanup asynchronously
        Console.WriteLine("🛑 Stopping polling...");
        poller.StopPolling();

        Console.WriteLine("✅ Test completed. Check console logs for expected results.");
    }
}