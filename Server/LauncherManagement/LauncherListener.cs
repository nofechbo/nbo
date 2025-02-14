using System;
using System.Collections.Generic;
using MyRPS;

namespace LauncherManagement
{
    public class LauncherListener
    {
        private readonly IRpsCommandHandler _rps;
        public event Action<string> OnMalfunctionDetected;
        private readonly List<Launcher> _launchers = new();

        public LauncherListener(IRpsCommandHandler rps)
        {
            _rps = rps;
        }

        public void RegisterLauncher(Launcher launcher)
        {
            _launchers.Add(launcher);
            launcher.MalfunctionOccurred += HandleMalfunction;
        }

        private void HandleMalfunction(string launcherCode)
        {
            Task.Run(async () => await HandleMalfunctionAsync(launcherCode));
        }

        private async Task HandleMalfunctionAsync(string launcherCode)
        {
            Console.WriteLine($"🚨 Malfunction detected on launcher {launcherCode}. Sending technician request...");
            string command = $"SendTechnician:{launcherCode}, Error579";
            await _rps.HandleRequestAsync(command);
            // Fire event for additional listeners
            OnMalfunctionDetected?.Invoke(command);
        }
    }
}
