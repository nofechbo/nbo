using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using LauncherManagement;
using DataBase;

public class LauncherTest
{
    public static async Task Main(string[] args)
    {
        // Set up Dependency Injection
        ServiceCollection services = new ServiceCollection();
        services.AddSingleton<MissileDbContext>();
        services.AddSingleton<DatabaseHandler>();
        services.AddSingleton<LauncherListener>();

        using var serviceProvider = services.BuildServiceProvider();
        LauncherListener launcherListener = serviceProvider.GetRequiredService<LauncherListener>();
        DatabaseHandler dbHandler = serviceProvider.GetRequiredService<DatabaseHandler>();

        Console.WriteLine("🔄 System initialized. Starting launcher setup...");

        // Register the first launcher
        Launcher launcher1 = new Launcher("L001", "Base B", "Type-Y", dbHandler);
        launcher1.RegisterNewLauncher();
        launcherListener.RegisterLauncher(launcher1);
        await Task.Delay(500);

        // Register the second launcher
        Launcher launcher2 = new Launcher("L002", "Base AB", "Type-X", dbHandler);
        launcher2.RegisterNewLauncher();
        launcherListener.RegisterLauncher(launcher2);
        await Task.Delay(500);

        // Simulate malfunctions with longer delays to allow TCP operations to complete
        launcher1.AlertMalfunction();
        await Task.Delay(3000);  // Wait longer for first TCP operation

        launcher2.AlertMalfunction();
        await Task.Delay(3000);  // Wait longer for second TCP operation

        
    }
}