using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using mainServer;
using DataBase;

namespace LauncherManagement
{
    public class LauncherPoller
    {
        private readonly LauncherListener _listener;
        private readonly HashSet<string> _trackedLaunchers = new();
        private bool _isPolling;
        private Task _pollingTask;
        private readonly int _pollingIntervalMs = 500;

        public LauncherPoller(LauncherListener listener)
        {
            _listener = listener ?? throw new ArgumentNullException(nameof(listener));
        }

        public void StartPolling()
        {
            if (_isPolling) return; // Prevent multiple instances

            _isPolling = true;
            _pollingTask = Task.Run(() => PollLaunchers());
            Console.WriteLine("📡 Launcher polling started...");
        }

        public void StopPolling()
        {
            _isPolling = false;
            _pollingTask?.Wait();
            Console.WriteLine("📡 Launcher polling stopped.");
        }

        private void PollLaunchers()
        {
            while (_isPolling)
            {
                try
                {
                    FetchAndRegisterLaunchers();
                    Thread.Sleep(_pollingIntervalMs); // Wait before polling again
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ Polling error: {ex.Message}");
                }
            }
        }

        private void FetchAndRegisterLaunchers()
        {
            using (var db = new MissileDbContext())
            {
                var newLaunchers = db.MissileLaunchers
                    .Select(l => new Launcher(l.Code, l.Location, l.MissileType))
                    .ToList();

                foreach (var launcher in newLaunchers)
                {
                    if (!_trackedLaunchers.Contains(launcher.Code)) 
                    {
                        _trackedLaunchers.Add(launcher.Code);
                        _listener.RegisterLauncher(launcher);
                        Console.WriteLine($"📡 New launcher registered: {launcher.Code}");
                    }
                }
            }
        }
    }
}


/*

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace mainServer
{
    public class LauncherPoller
    {
        private readonly List<MissileLauncher> _launchers = new();
        private readonly LauncherListener _listener;
        private readonly object _lock = new();
        private bool _isPolling;
        private Thread _pollingThread;

        public LauncherPoller(LauncherListener listener)
        {
            _listener = listener ?? throw new ArgumentNullException(nameof(listener));
        }

        public void AddLauncher(MissileLauncher launcher)
        {
            if (launcher == null) throw new ArgumentNullException(nameof(launcher));

            lock (_lock)
            {
                _launchers.Add(launcher);
                _listener.RegisterLauncher(launcher);
                Console.WriteLine($"✅ Launcher {launcher.Code} registered for tracking.");
            }
        }

        public void StartPolling()
        {
            if (_isPolling)
                return; // Already running

            _isPolling = true;
            _pollingThread = new Thread(() =>
            {
                while (_isPolling)
                {
                    lock (_lock)
                    {
                        foreach (var launcher in _launchers)
                        {
                            if (launcher.HasMalfunction)  // Simulated check
                            {
                                Console.WriteLine($"🚨 Malfunction detected in {launcher.Code}, alerting listener...");
                                launcher.AlertMalfunction();
                            }
                        }
                    }

                    Thread.Sleep(1000); // Poll every second
                }
            })
            {
                IsBackground = true // Ensures it stops when the main program exits
            };

            _pollingThread.Start();
        }

        public void StopPolling()
        {
            _isPolling = false;
            _pollingThread?.Join(); // Ensure the thread stops gracefully
        }

        public MissileLauncher GetLauncher(string code)
        {
            lock (_lock)
            {
                return _launchers.Find(l => l.Code == code);
            }
        }
    }
}


*/