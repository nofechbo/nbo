using System;
using System.Collections.Generic;
using Launcher;
using mainServer;
using Launcher;

namespace LauncherManagement
{
    public class LauncherListener
    {
        private readonly IRpsCommandHandler _rps;
        public event Action<string> OnMalfunctionDetected;
        private readonly List<MissileLauncher> _launchers = new();

        // Inject RPS via Dependency Injection
        public LauncherListener(IRpsCommandHandler rps)
        {
            _rps = rps;
        }

        public void RegisterLauncher(MissileLauncher launcher)
        {
            _launchers.Add(launcher);
            launcher.MalfunctionOccurred += HandleMalfunction;
        }

        private void HandleMalfunction(string launcherCode)
        {
            Console.WriteLine($"🚨 Malfunction detected on launcher {launcherCode}. Sending technician request...");
            string command = $"SendTechnician:{launcherCode}";

            // Use RPS to execute the command
            _rps.ExecuteCommand(command);

            // Fire event for additional listeners
            OnMalfunctionDetected?.Invoke(command);
        }
    }
}
