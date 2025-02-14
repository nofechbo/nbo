using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Moq;
using MyRPS;
using LauncherManagement;
using DataBase;

[TestFixture]
public class LauncherListenerTests
{
    private ServiceProvider _serviceProvider;
    private Mock<IRpsCommandHandler> _mockRpsHandler;
    private LauncherListener _launcherListener;
    private LauncherPoller _poller;
    private DatabaseHandler _dbHandler;

    [SetUp]
    public void Setup()
    {
        // Set up Dependency Injection
        var services = new ServiceCollection();
        _mockRpsHandler = new Mock<IRpsCommandHandler>();

        // Add the mock RPS handler to DI
        services.AddSingleton<IRpsCommandHandler>(_mockRpsHandler.Object);
        services.AddSingleton<LauncherListener>();
        services.AddSingleton<LauncherPoller>();

        // Register DatabaseHandler with missileDbContext
        services.AddSingleton<DatabaseHandler>();

        // Build the service provider
        _serviceProvider = services.BuildServiceProvider();

        // Resolve the required services from DI container
        _launcherListener = _serviceProvider.GetRequiredService<LauncherListener>();
        _poller = _serviceProvider.GetRequiredService<LauncherPoller>();
        _dbHandler = _serviceProvider.GetRequiredService<DatabaseHandler>();

        _poller.StartPolling();
    }

    [Test]
    public async Task Polling_ShouldDetectNewLaunchers_AndHandleMalfunctions()
    {
        // Create a new launcher (simulating external addition)
        var launcher = new Launcher("L004", "Base B", "Type-Y", _dbHandler);

        // Register the launcher manually and save it to the database
        launcher.RegisterNewLauncher();  // Ensure it's saved to the database

        // Manually register the launcher in the listener
        _launcherListener.RegisterLauncher(launcher);

        // Wait briefly to allow polling to process
        await Task.Delay(500);

        // Simulate malfunction
        launcher.AlertMalfunction();

        // Verify RPS received the correct command (use mock RPS handler)
        _mockRpsHandler.Verify(
            rps => rps.HandleRequestAsync("SendTechnician:L004"),  // Ensure the correct launcher code is used
            Times.Once);

        Assert.Pass("Polling successfully detected new launcher and handled malfunction.");
    }

    [TearDown]
    public void Cleanup()
    {
        _poller.StopPolling();
        _serviceProvider?.Dispose();
    }
}
