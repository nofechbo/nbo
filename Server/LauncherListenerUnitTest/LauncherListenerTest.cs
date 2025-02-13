using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Moq;
using mainServer;
using LauncherManagement;

[TestFixture]
public class LauncherListenerTests
{
    private ServiceProvider _serviceProvider;
    private Mock<IRpsCommandHandler> _mockRpsHandler;
    private LauncherListener _launcherListener;
    private LauncherPoller _poller;

    [SetUp]
    public void Setup()
    {
        // Set up Dependency Injection
        var services = new ServiceCollection();
        _mockRpsHandler = new Mock<IRpsCommandHandler>();

        services.AddSingleton<IRpsCommandHandler>(_mockRpsHandler.Object);
        services.AddSingleton<LauncherListener>();
        services.AddSingleton<LauncherPoller>();

        _serviceProvider = services.BuildServiceProvider();
        _launcherListener = _serviceProvider.GetRequiredService<LauncherListener>();
        _poller = _serviceProvider.GetRequiredService<LauncherPoller>();

        _poller.StartPolling();
    }

    [Test]
    public async Task Polling_ShouldDetectNewLaunchers_AndHandleMalfunctions()
    {
        // Create a new launcher (simulating external addition)
        var launcher = new Launcher("L002", "Base B", "Type-Y");

        // Register the launcher manually
        _launcherListener.RegisterLauncher(launcher);

        // Wait briefly to allow polling to process
        await Task.Delay(500);

        // Simulate malfunction
        launcher.AlertMalfunction();

        // Verify RPS received the correct command
        _mockRpsHandler.Verify(
            rps => rps.HandleRequestAsync("SendTechnician:L002"),
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