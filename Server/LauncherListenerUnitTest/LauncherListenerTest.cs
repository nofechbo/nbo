using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Moq;
using mainServer;
using Launcher;
using LauncherManagement;

[TestFixture]
public class LauncherListenerTests
{
    private ServiceProvider _serviceProvider;
    private Mock<IRpsCommandHandler> _mockRpsHandler;
    private LauncherListener _launcherListener;
    private LauncherPoller _poller;
    private CancellationTokenSource _cts;

    [SetUp]
    public void Setup()
    {
        // ✅ Set up Dependency Injection
        var services = new ServiceCollection();
        _mockRpsHandler = new Mock<IRpsCommandHandler>();

        services.AddSingleton<IRpsCommandHandler>(_mockRpsHandler.Object);
        services.AddSingleton<LauncherListener>();
        services.AddSingleton<LauncherPoller>();

        _serviceProvider = services.BuildServiceProvider();
        _launcherListener = _serviceProvider.GetRequiredService<LauncherListener>();
        _poller = _serviceProvider.GetRequiredService<LauncherPoller>();

        // ✅ Start polling in the background
        _cts = new CancellationTokenSource();
        Task.Run(() => _poller.StartPolling(_cts.Token));
    }

    [Test]
    public async Task Polling_ShouldDetectNewLaunchers_AndHandleMalfunctions()
    {
        // ✅ Create a new launcher (simulating external addition)
        var launcher = new MissileLauncher("L002", "Base B", "Type-Y");

        // ✅ Simulate polling detecting the new launcher
        _poller.AddLauncher(launcher); // This should be a thread-safe method in LauncherPoller

        // ✅ Wait a moment to let the polling cycle detect it
        await Task.Delay(500);

        // ✅ Simulate malfunction
        launcher.AlertMalfunction();

        // ✅ Verify RPS received the correct command
        _mockRpsHandler.Verify(
            rps => rps.HandleRequestAsync("SendTechnician:L002"),
            Times.Once);

        Assert.Pass("Polling successfully detected new launcher and handled malfunction.");
    }

    [TearDown]
    public void Cleanup()
    {
        _cts.Cancel(); // ✅ Stop the polling loop
        _serviceProvider?.Dispose();
    }
}
